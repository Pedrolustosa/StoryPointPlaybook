namespace StoryPointPlaybook.Application.Interfaces;

public interface IGameHubNotifier
{
    Task NotifyUserVoted(Guid roomId, Guid userId);
    Task NotifyVotesRevealed(Guid roomId);
    Task NotifyStoryAdded(Guid roomId, object storyDto);
}
