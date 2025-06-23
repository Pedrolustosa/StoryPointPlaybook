using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Infrastructure.Repositories;

namespace StoryPointPlaybook.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly PlanningPokerContext _context;

    // Lazy loading dos repositórios
    private IRoomRepository? _rooms;
    private IStoryRepository? _stories;
    private IVoteRepository? _votes;
    private IUserRepository? _users;
    private IChatMessageRepository? _chatMessages;
    private ISessionRepository? _sessions;

    public UnitOfWork(PlanningPokerContext context)
    {
        _context = context;
    }

    // Propriedades dos repositórios com lazy loading
    public IRoomRepository Rooms => _rooms ??= new RoomRepository(_context);
    public IStoryRepository Stories => _stories ??= new StoryRepository(_context);
    public IVoteRepository Votes => _votes ??= new VoteRepository(_context);
    public IUserRepository Users => _users ??= new UserRepository(_context);
    public IChatMessageRepository ChatMessages => _chatMessages ??= new ChatMessageRepository(_context);
    public ISessionRepository Sessions => _sessions ??= new SessionRepository(_context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
