using StoryPointPlaybook.Application.DTOs;

namespace StoryPointPlaybook.Application.Interfaces;

public interface IGameHubNotifier
{
    Task NotifyUserVoted(Guid roomId, Guid userId);
    Task NotifyVotesRevealed(Guid roomId);
    Task NotifyStoryAdded(Guid roomId, object storyDto);
    Task NotifyStoryAdded(string roomCode, StoryResponse storyDto);
    Task NotifyCurrentStoryChanged(string roomCode, StoryResponse storyDto);
    Task NotifyVotesRevealedWithResults(Guid roomId, IEnumerable<VoteResultDto> votes);
    Task NotifyVotingStatusUpdated(Guid roomId, List<VotingStatusDto> statusList);
}
