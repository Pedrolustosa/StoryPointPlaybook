using StoryPointPlaybook.API.Hubs;
using Microsoft.AspNetCore.SignalR;
using StoryPointPlaybook.Application.Interfaces;

namespace StoryPointPlaybook.API.SignalR;

public class ChatHubNotifier(IHubContext<ChatHub> hub) : IChatHubNotifier
{
    private readonly IHubContext<ChatHub> _hub = hub;

    public async Task NotifyMessageSent(Guid roomId, string userName, string message, DateTime timestamp)
    {
        await _hub.Clients.Group(roomId.ToString()).SendAsync("ChatMessage", new
        {
            UserName = userName,
            Message = message,
            Timestamp = timestamp
        });
    }
}