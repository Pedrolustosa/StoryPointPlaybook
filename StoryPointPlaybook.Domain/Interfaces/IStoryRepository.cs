using StoryPointPlaybook.Domain.Entities;

namespace StoryPointPlaybook.Domain.Interfaces;

public interface IStoryRepository
{
    Task<Story?> GetByIdAsync(Guid id);
    Task<List<Story>> GetByRoomIdAsync(Guid roomId);
    Task AddAsync(Story story);
    Task UpdateAsync(Story story);
    Task DeleteAsync(Story story);
    Task<Story?> GetByIdWithRoomAsync(Guid storyId);
}
