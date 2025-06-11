using MediatR;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Domain.Exceptions;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Application.CQRS.Commands;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class SubmitVoteHandler(
    IStoryRepository storyRepo,
    IUserRepository userRepo,
    IVoteRepository voteRepo,
    IGameHubNotifier notifier,
    IUnitOfWork unitOfWork) : IRequestHandler<SubmitVoteCommand>
{
    private readonly IStoryRepository _storyRepo = storyRepo;
    private readonly IUserRepository _userRepo = userRepo;
    private readonly IVoteRepository _voteRepo = voteRepo;
    private readonly IGameHubNotifier _notifier = notifier;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(SubmitVoteCommand request, CancellationToken cancellationToken)
    {
        var story = await _storyRepo.GetByIdWithRoomAsync(request.StoryId)
            ?? throw new StoryNotFoundException();
        var user = await _userRepo.GetByIdAsync(request.UserId)
            ?? throw new UserNotFoundException();
        var existingVote = story.Votes.FirstOrDefault(v => v.UserId == user.Id);
        if (existingVote != null)
        {
            existingVote.SetValue(request.Value);
            await _voteRepo.UpdateAsync(existingVote);
            await _unitOfWork.SaveChangesAsync();
        }
        else
        {
            var vote = new Vote(request.StoryId, user.Id, request.Value);
            await _voteRepo.AddAsync(vote);
            await _unitOfWork.SaveChangesAsync();
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
            await _unitOfWork.SaveChangesAsync();
            await _notifier.NotifyVotesRevealed(story.Room.Id);
        }
    }
}

