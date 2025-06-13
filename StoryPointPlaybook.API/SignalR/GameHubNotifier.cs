using StoryPointPlaybook.API.Hubs;
using Microsoft.AspNetCore.SignalR;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Application.Interfaces;

namespace StoryPointPlaybook.API.SignalR;

public class GameHubNotifier(IHubContext<GameHub> hub) : IGameHubNotifier
{
    private readonly IHubContext<GameHub> _hub = hub;

    public async Task NotifyUserVoted(Guid roomId, Guid userId)
    {
        await _hub.Clients.Group(roomId.ToString())
            .SendAsync("VoteReceived", userId, true);
    }

    public async Task NotifyVotesRevealed(Guid roomId)
    {
        await _hub.Clients.Group(roomId.ToString())
            .SendAsync("VotesRevealed");
    }

    public async Task NotifyStoryAdded(Guid roomId, object storyDto)
    {
        await _hub.Clients.Group(roomId.ToString())
            .SendAsync("StoryUpdated", storyDto);
    }

    public async Task NotifyStoryAdded(string roomCode, StoryResponse storyDto)
    {
        await _hub.Clients.Group(roomCode)
            .SendAsync("StoryAdded", storyDto);
    }
    public async Task NotifyCurrentStoryChanged(string roomCode, StoryResponse storyDto)
    {
        await _hub.Clients.Group(roomCode).SendAsync("CurrentStoryChanged", storyDto);
    }

    public async Task NotifyVotesRevealedWithResults(Guid roomId, IEnumerable<VoteResultDto> votes)
    {
        await _hub.Clients.Group(roomId.ToString())
            .SendAsync("VotesRevealedWithResults", votes);
    }

    public async Task NotifyVotingStatusUpdated(Guid roomId, List<VotingStatusDto> statusList)
    {
        await _hub.Clients.Group(roomId.ToString())
            .SendAsync("VotingStatusUpdated", statusList);
    }
}
