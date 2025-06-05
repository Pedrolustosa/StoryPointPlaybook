using MediatR;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;

public class SubmitVoteHandler : IRequestHandler<SubmitVoteCommand, Unit>
{
    private readonly IVoteRepository _voteRepo;
    private readonly IStoryRepository _storyRepo;
    private readonly IGameHubNotifier _notifier;

    public SubmitVoteHandler(
        IVoteRepository voteRepo,
        IStoryRepository storyRepo,
        IGameHubNotifier notifier)
    {
        _voteRepo = voteRepo;
        _storyRepo = storyRepo;
        _notifier = notifier;
    }

    public async Task<Unit> Handle(SubmitVoteCommand request, CancellationToken cancellationToken)
    {
        var vote = new Vote(request.StoryId, request.UserId, request.Value);
        await _voteRepo.AddAsync(vote);

        var story = await _storyRepo.GetByIdAsync(request.StoryId);
        if (story != null)
            await _notifier.NotifyUserVoted(story.RoomId, request.UserId);

        return Unit.Value;
    }
}
