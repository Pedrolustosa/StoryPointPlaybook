using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StoryPointPlaybook.Api.Hubs;
using StoryPointPlaybook.API.SignalR;
using StoryPointPlaybook.Application.CQRS.Rooms.Commands;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Application.Services;
using StoryPointPlaybook.Application.Validators;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Infrastructure.Data;
using StoryPointPlaybook.Infrastructure.Repositories;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

#region Logger (Serilog)
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();
#endregion

#region Serviços e Injeção de Dependência

// DbContext
builder.Services.AddDbContext<PlanningPokerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .WithOrigins(
                  "http://localhost:4200",
                  "https://localhost:4200",
                  "http://localhost:8080"
              );
    });
});

// Repositórios
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStoryRepository, StoryRepository>();
builder.Services.AddScoped<IVoteRepository, VoteRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();

// SignalR e Notificações
builder.Services.AddSignalR();
builder.Services.AddScoped<IGameHubNotifier, GameHubNotifier>();
builder.Services.AddSingleton<IConnectedUserTracker, ConnectedUserTracker>();
builder.Services.AddScoped<IChatHubNotifier, ChatHubNotifier>();

// Rate limiting
builder.Services.AddSingleton<IRateLimitService, InMemoryRateLimitService>();

// MediatR + Pipeline de validação
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateRoomCommand).Assembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddOptionsWithValidateOnStart<CreateRoomCommand>();

// Controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

#region Rate Limiter (global)
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 5,
            Window = TimeSpan.FromSeconds(10),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 2
        });
    });

    options.RejectionStatusCode = 429;
});
#endregion

var app = builder.Build();
app.UseCors("AllowLocalhost");
#region Middleware de tratamento de exceções
app.UseExceptionHandler(config =>
{
    config.Run(async context =>
    {
        context.Response.StatusCode = 400;
        context.Response.ContentType = "application/json";

        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (ex is ValidationException ve)
        {
            var result = new
            {
                Errors = ve.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            };
            await context.Response.WriteAsJsonAsync(result);
        }
    });
});
#endregion

#region Pipeline da aplicação

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthorization();

app.MapControllers();
app.MapHub<GameHub>("/gamehub");
app.MapHub<ChatHub>("/chathub");

#endregion

app.Run();
