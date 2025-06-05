using StoryPointPlaybook.Domain.Entities;

namespace StoryPointPlaybook.Domain.Interfaces;

public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(Guid id);
    Task<List<Session>> GetByRoomIdAsync(Guid roomId);
    Task AddAsync(Session session);
    Task UpdateAsync(Session session);
}
