using Microsoft.AspNetCore.SignalR;
using StoryPointPlaybook.Api.Hubs;
using StoryPointPlaybook.Application.Interfaces;

namespace StoryPointPlaybook.API.SignalR
{
    public class GameHubNotifier : IGameHubNotifier
    {
        private readonly IHubContext<GameHub> _hub;

        public GameHubNotifier(IHubContext<GameHub> hub)
        {
            _hub = hub;
        }

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
    }
}
