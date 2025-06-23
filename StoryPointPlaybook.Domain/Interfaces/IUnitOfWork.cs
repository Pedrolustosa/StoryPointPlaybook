namespace StoryPointPlaybook.Domain.Interfaces;

public interface IUnitOfWork
{
    // Repositórios
    IRoomRepository Rooms { get; }
    IStoryRepository Stories { get; }
    IVoteRepository Votes { get; }
    IUserRepository Users { get; }
    IChatMessageRepository ChatMessages { get; }
    ISessionRepository Sessions { get; }

    // Operações de transação
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
