using Microsoft.EntityFrameworkCore;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Infrastructure.Data;

namespace StoryPointPlaybook.Infrastructure.Repositories;

public class UserRepository(PlanningPokerContext context) : IUserRepository
{
    private readonly PlanningPokerContext _context = context;

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
    }

    public async Task<List<User>> GetByRoomIdAsync(Guid roomId)
    {
        return await _context.Users
            .Where(u => u.RoomId == roomId)
            .ToListAsync();
    }
}
