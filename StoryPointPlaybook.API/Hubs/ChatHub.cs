using Microsoft.AspNetCore.SignalR;

namespace StoryPointPlaybook.Api.Hubs;

public class ChatHub : Hub
{
    // Chamado pelo frontend ao entrar na sala
    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
    }

    // Chamado pelo frontend ao sair da sala
    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
    }

    // Envia mensagem para todos na sala
    public async Task SendMessage(string roomId, string user, string message)
    {
        var timestamp = DateTime.UtcNow.ToString("HH:mm:ss");
        await Clients.Group(roomId).SendAsync("ChatMessage", user, message, timestamp);
    }
}
