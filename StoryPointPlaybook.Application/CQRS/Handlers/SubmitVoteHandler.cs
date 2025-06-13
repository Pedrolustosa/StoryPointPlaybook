using MediatR;
using StoryPointPlaybook.Application.CQRS.Commands;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Application.Events;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Exceptions;
using StoryPointPlaybook.Domain.Interfaces;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class SubmitVoteHandler(
    IStoryRepository storyRepo,
    IUserRepository userRepo,
    IVoteRepository voteRepo,
    IUnitOfWork unitOfWork,
    IGameHubNotifier notifier,
    IMediator mediator) : IRequestHandler<SubmitVoteCommand>
{
    private readonly IStoryRepository _storyRepo = storyRepo;
    private readonly IUserRepository _userRepo = userRepo;
    private readonly IVoteRepository _voteRepo = voteRepo;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IGameHubNotifier _notifier = notifier;
    private readonly IMediator _mediator = mediator;

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
        }
        else
        {
            var vote = new Vote(request.StoryId, user.Id, request.Value);
            await _voteRepo.AddAsync(vote);
        }

        await _unitOfWork.SaveChangesAsync();

        var voters = story.Room.Participants
            .Where(p => !string.Equals(p.Role, "PO", StringComparison.OrdinalIgnoreCase))
            .ToList();

        var totalVotes = story.Votes
            .Count(v => !string.IsNullOrWhiteSpace(v.Value) &&
                        voters.Any(p => p.Id == v.UserId));

        var votesRevealed = false;

        if (story.Room.AutoReveal && totalVotes == voters.Count)
        {
            story.RevealVotes();
            await _storyRepo.UpdateAsync(story);
            await _unitOfWork.SaveChangesAsync();
            votesRevealed = true;
        }

        var votingStatus = voters.Select(p => new VotingStatusDto
        {
            UserId = p.Id,
            DisplayName = p.Name,
            HasVoted = story.Votes.Any(v => v.UserId == p.Id && !string.IsNullOrWhiteSpace(v.Value))
        }).ToList();

        await _notifier.NotifyVotingStatusUpdated(story.Room.Id, votingStatus);

        await _mediator.Publish(new VoteSubmittedEvent(story.Room.Id, user.Id, votesRevealed), cancellationToken);
    }
}