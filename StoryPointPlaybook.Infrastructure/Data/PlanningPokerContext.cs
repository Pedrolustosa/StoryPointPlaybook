using Microsoft.EntityFrameworkCore;
using StoryPointPlaybook.Domain.Entities;

namespace StoryPointPlaybook.Infrastructure.Data;

public class PlanningPokerContext(DbContextOptions<PlanningPokerContext> options) : DbContext(options)
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Story> Stories => Set<Story>();
    public DbSet<Vote> Votes => Set<Vote>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PlanningPokerContext).Assembly);
    }
}
