using MediatR;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;
using StoryPointPlaybook.Domain.Interfaces;

public class RevealVotesHandler : IRequestHandler<RevealVotesCommand, Unit>
{
    private readonly IStoryRepository _storyRepo;
    private readonly IGameHubNotifier _notifier;

    public RevealVotesHandler(IStoryRepository storyRepo, IGameHubNotifier notifier)
    {
        _storyRepo = storyRepo;
        _notifier = notifier;
    }

    public async Task<Unit> Handle(RevealVotesCommand request, CancellationToken cancellationToken)
    {
        var story = await _storyRepo.GetByIdAsync(request.StoryId);
        if (story == null)
            throw new InvalidOperationException("História não encontrada");

        story.RevealVotes();
        await _storyRepo.UpdateAsync(story);

        await _notifier.NotifyVotesRevealed(story.RoomId);

        return Unit.Value;
    }
}
