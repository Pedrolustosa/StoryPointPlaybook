using StoryPointPlaybook.Domain.Entities;

namespace StoryPointPlaybook.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User story);
    Task UpdateAsync(User story);
    Task<List<User>> GetByRoomIdAsync(Guid roomId);

}
