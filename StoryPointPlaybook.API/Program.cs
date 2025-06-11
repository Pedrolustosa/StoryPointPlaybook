using Serilog;
using MediatR;
using FluentValidation;
using StoryPointPlaybook.API.Hubs;
using System.Threading.RateLimiting;
using StoryPointPlaybook.API.SignalR;
using Microsoft.AspNetCore.Diagnostics;
using StoryPointPlaybook.API.Common;
using StoryPointPlaybook.Application.Services;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Application.Validators;
using StoryPointPlaybook.Application.CQRS.Rooms.Commands;
using StoryPointPlaybook.Infra.IoC;
using StoryPointPlaybook.Infrastructure.Data;

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

// Infraestrutura
builder.Services.AddInfrastructure(builder.Configuration);

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
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(ms => ms.Value?.Errors.Count > 0)
                .SelectMany(kvp => kvp.Value!.Errors
                    .Select(e => $"{kvp.Key}: {e.ErrorMessage}"))
                .ToList();
            var response = ApiResponse<string>.ErrorResponse("Erro de validação.", errors);
            return new BadRequestObjectResult(response);
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddDbContextCheck<PlanningPokerContext>();

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
        context.Response.ContentType = "application/json";

        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (ex is ValidationException ve)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var errors = ve.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToList();
            var result = ApiResponse<string>.ErrorResponse("Erro de validação.", errors);
            await context.Response.WriteAsJsonAsync(result);
        }
        else if (ex is not null)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var result = ApiResponse<string>.ErrorResponse("Erro interno do servidor.");
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
app.MapHealthChecks("/health");

#endregion

app.Run();
