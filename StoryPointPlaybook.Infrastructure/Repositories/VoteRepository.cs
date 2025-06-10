using Microsoft.EntityFrameworkCore;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Infrastructure.Data;

namespace StoryPointPlaybook.Infrastructure.Repositories;

public class VoteRepository(PlanningPokerContext context) : IVoteRepository
{
    private readonly PlanningPokerContext _context = context;

    public async Task<List<Vote>> GetByStoryIdAsync(Guid storyId)
    {
        return await _context.Votes
            .Where(v => v.StoryId == storyId)
            .Include(v => v.User)
            .ToListAsync();
    }

    public async Task<Vote?> GetByUserAndStoryAsync(Guid userId, Guid storyId)
    {
        return await _context.Votes
            .FirstOrDefaultAsync(v => v.UserId == userId && v.StoryId == storyId);
    }

    public async Task AddAsync(Vote vote)
    {
        await _context.Votes.AddAsync(vote);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vote vote)
    {
        _context.Votes.Update(vote);
        await _context.SaveChangesAsync();
    }
}
