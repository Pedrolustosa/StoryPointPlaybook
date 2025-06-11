using MediatR;
using StoryPointPlaybook.Application.Events;
using StoryPointPlaybook.Application.Interfaces;

namespace StoryPointPlaybook.Application.EventHandlers;

public class StoryAddedEventHandler(IGameHubNotifier notifier) : INotificationHandler<StoryAddedEvent>
{
    private readonly IGameHubNotifier _notifier = notifier;

    public async Task Handle(StoryAddedEvent notification, CancellationToken cancellationToken)
    {
        await _notifier.NotifyStoryAdded(notification.RoomCode, notification.Story);
    }
}
