using Microsoft.AspNetCore.SignalR;
using StoryPointPlaybook.Api.Hubs;
using StoryPointPlaybook.Application.Interfaces;

public class ChatHubNotifier : IChatHubNotifier
{
    private readonly IHubContext<ChatHub> _hub;

    public ChatHubNotifier(IHubContext<ChatHub> hub)
    {
        _hub = hub;
    }

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
