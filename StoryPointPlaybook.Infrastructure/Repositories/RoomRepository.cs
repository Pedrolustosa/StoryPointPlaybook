using Microsoft.EntityFrameworkCore;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Infrastructure.Data;

namespace StoryPointPlaybook.Infrastructure.Repositories;

public class RoomRepository(PlanningPokerContext context) : IRoomRepository
{
    private readonly PlanningPokerContext _context = context;

    public async Task<Room?> GetByIdAsync(Guid id)
    {
        return await _context.Rooms
            .Include(r => r.Participants)
            .Include(r => r.Stories)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Room?> GetByCodeAsync(string code)
    {
        return await _context.Rooms
            .Include(r => r.Participants)
            .Include(r => r.Stories)
            .FirstOrDefaultAsync(r => r.Code == code);
    }

    public async Task AddAsync(Room room)
    {
        await _context.Rooms.AddAsync(room);
    }

    public async Task UpdateAsync(Room room)
    {
        _context.Rooms.Update(room);
    }

    public async Task DeleteAsync(Room room)
    {
        _context.Rooms.Remove(room);
    }
}
