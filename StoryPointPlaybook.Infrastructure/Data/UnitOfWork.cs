using StoryPointPlaybook.Domain.Interfaces;

namespace StoryPointPlaybook.Infrastructure.Data;

public class UnitOfWork(PlanningPokerContext context) : IUnitOfWork
{
    private readonly PlanningPokerContext _context = context;

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}

