using StoryPointPlaybook.Domain.Entities;

namespace StoryPointPlaybook.Domain.Interfaces;

public interface IChatMessageRepository
{
    Task AddAsync(ChatMessage message);
    Task<List<ChatMessage>> GetByRoomIdAsync(Guid roomId);
}
