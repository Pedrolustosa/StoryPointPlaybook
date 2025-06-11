using Microsoft.EntityFrameworkCore;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Infrastructure.Data;

namespace StoryPointPlaybook.Infrastructure.Repositories;

public class SessionRepository(PlanningPokerContext context) : ISessionRepository
{
    private readonly PlanningPokerContext _context = context;

    public async Task<Session?> GetByIdAsync(Guid id)
    {
        return await _context.Sessions
            .Include(s => s.Room)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Session>> GetByRoomIdAsync(Guid roomId)
    {
        return await _context.Sessions
            .Where(s => s.RoomId == roomId)
            .ToListAsync();
    }

    public async Task AddAsync(Session session)
    {
        await _context.Sessions.AddAsync(session);
    }

    public async Task UpdateAsync(Session session)
    {
        _context.Sessions.Update(session);
    }
}
