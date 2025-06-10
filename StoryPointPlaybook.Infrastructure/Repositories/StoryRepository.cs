using Microsoft.EntityFrameworkCore;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Infrastructure.Data;

namespace StoryPointPlaybook.Infrastructure.Repositories;

public class StoryRepository(PlanningPokerContext context) : IStoryRepository
{
    private readonly PlanningPokerContext _context = context;

    public async Task<Story?> GetByIdAsync(Guid id)
    {
        return await _context.Stories
            .Include(s => s.Votes)
            .ThenInclude(v => v.User)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Story>> GetByRoomIdAsync(Guid roomId)
    {
        return await _context.Stories
            .Where(s => s.RoomId == roomId)
            .Include(s => s.Votes)
            .ToListAsync();
    }

    public async Task AddAsync(Story story)
    {
        await _context.Stories.AddAsync(story);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Story story)
    {
        _context.Stories.Update(story);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Story story)
    {
        _context.Stories.Remove(story);
        await _context.SaveChangesAsync();
    }

    public async Task<Story?> GetByIdWithRoomAsync(Guid storyId)
    {
        return await _context.Stories
            .Include(s => s.Room)
                .ThenInclude(r => r.Participants)
            .Include(s => s.Votes)
            .FirstOrDefaultAsync(s => s.Id == storyId);
    }
}
