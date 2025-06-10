namespace StoryPointPlaybook.Application.Interfaces;

public interface IChatHubNotifier
{
    Task NotifyMessageSent(Guid roomId, string userName, string message, DateTime timestamp);
}
