using MediatR;
using StoryPointPlaybook.Application.Events;
using StoryPointPlaybook.Application.Interfaces;

namespace StoryPointPlaybook.Application.EventHandlers;

public class VoteSubmittedEventHandler(IGameHubNotifier notifier) : INotificationHandler<VoteSubmittedEvent>
{
    private readonly IGameHubNotifier _notifier = notifier;

    public async Task Handle(VoteSubmittedEvent notification, CancellationToken cancellationToken)
    {
        await _notifier.NotifyUserVoted(notification.RoomId, notification.UserId);
        if (notification.VotesRevealed)
        {
            await _notifier.NotifyVotesRevealed(notification.RoomId);
        }
    }
}
