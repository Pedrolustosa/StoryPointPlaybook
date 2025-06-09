using MediatR;
using StoryPointPlaybook.Application.Common;
using StoryPointPlaybook.Application.CQRS.Commands;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class SubmitVoteHandler : IRequestHandler<SubmitVoteCommand>
{
    private readonly IStoryRepository _storyRepo;
    private readonly IUserRepository _userRepo;
    private readonly IVoteRepository _voteRepo;
    private readonly IGameHubNotifier _notifier;

    public SubmitVoteHandler(
        IStoryRepository storyRepo,
        IUserRepository userRepo,
        IVoteRepository voteRepo,
        IGameHubNotifier notifier)
    {
        _storyRepo = storyRepo;
        _userRepo = userRepo;
        _voteRepo = voteRepo;
        _notifier = notifier;
    }

    public async Task Handle(SubmitVoteCommand request, CancellationToken cancellationToken)
    {
        var story = await _storyRepo.GetByIdWithRoomAsync(request.StoryId);
        if (story == null)
            throw new Exception(ApplicationErrors.StoryNotFound);

        var user = await _userRepo.GetByIdAsync(request.UserId);
        if (user == null)
            throw new Exception(ApplicationErrors.UserNotFound);

        var existingVote = story.Votes.FirstOrDefault(v => v.UserId == user.Id);
        if (existingVote != null)
        {
            existingVote.SetValue(request.Value);
            await _voteRepo.UpdateAsync(existingVote);
        }
        else
        {
            var vote = new Vote(request.StoryId, user.Id, request.Value);
            await _voteRepo.AddAsync(vote);
        }

        await _notifier.NotifyUserVoted(story.Room.Id, user.Id);

        var voters = story.Room.Participants
            .Where(p => !string.Equals(p.Role, "PO", StringComparison.OrdinalIgnoreCase))
            .ToList();

        var totalVotes = story.Votes
            .Count(v => !string.IsNullOrWhiteSpace(v.Value) &&
                        voters.Any(p => p.Id == v.UserId));

        if (story.Room.AutoReveal && totalVotes == voters.Count)
        {
            story.RevealVotes();
            await _storyRepo.UpdateAsync(story);
            await _notifier.NotifyVotesRevealed(story.Room.Id);
        }
    }
}
