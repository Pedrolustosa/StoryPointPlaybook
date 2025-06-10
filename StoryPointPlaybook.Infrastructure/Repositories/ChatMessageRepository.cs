using Microsoft.EntityFrameworkCore;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Infrastructure.Data;

namespace StoryPointPlaybook.Infrastructure.Repositories;

public class ChatMessageRepository(PlanningPokerContext context) : IChatMessageRepository
{
    private readonly PlanningPokerContext _context = context;

    public async Task AddAsync(ChatMessage message)
    {
        await _context.ChatMessages.AddAsync(message);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ChatMessage>> GetByRoomIdAsync(Guid roomId)
    {
        return await _context.ChatMessages
            .Where(m => m.RoomId == roomId)
            .OrderBy(m => m.Timestamp)
            .ToListAsync();
    }
}
