using Microsoft.EntityFrameworkCore;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Infrastructure.Data;

namespace StoryPointPlaybook.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly PlanningPokerContext _context;

    public RoomRepository(PlanningPokerContext context)
    {
        _context = context;
    }

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
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Room room)
    {
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Room room)
    {
        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
    }
}
