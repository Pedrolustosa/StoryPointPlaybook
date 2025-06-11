using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoryPointPlaybook.Infrastructure.Data;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Infrastructure.Repositories;

namespace StoryPointPlaybook.Infra.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PlanningPokerContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IStoryRepository, StoryRepository>();
        services.AddScoped<IVoteRepository, VoteRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
