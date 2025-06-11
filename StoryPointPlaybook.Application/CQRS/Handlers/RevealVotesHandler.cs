using MediatR;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class RevealVotesHandler(IStoryRepository storyRepo, IGameHubNotifier notifier, IUnitOfWork unitOfWork) : IRequestHandler<RevealVotesCommand, Unit>
{
    private readonly IStoryRepository _storyRepo = storyRepo;
    private readonly IGameHubNotifier _notifier = notifier;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(RevealVotesCommand request, CancellationToken cancellationToken)
    {
        var story = await _storyRepo.GetByIdAsync(request.StoryId);
        if (story == null)
            throw new InvalidOperationException("História não encontrada");

        story.RevealVotes();
        await _storyRepo.UpdateAsync(story);
        await _unitOfWork.SaveChangesAsync();

        await _notifier.NotifyVotesRevealed(story.RoomId);

        return Unit.Value;
    }
}
