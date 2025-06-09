using StoryPointPlaybook.Domain.Entities;

namespace StoryPointPlaybook.Domain.Interfaces;

public interface IVoteRepository
{
    Task<List<Vote>> GetByStoryIdAsync(Guid storyId);
    Task<Vote?> GetByUserAndStoryAsync(Guid userId, Guid storyId);
    Task AddAsync(Vote vote);
    Task UpdateAsync(Vote vote);
}
