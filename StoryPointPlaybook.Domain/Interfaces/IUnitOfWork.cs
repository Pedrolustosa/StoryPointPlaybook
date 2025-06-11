namespace StoryPointPlaybook.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}

