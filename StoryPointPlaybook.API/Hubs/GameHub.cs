using Microsoft.AspNetCore.SignalR;
using StoryPointPlaybook.Application.Interfaces;

namespace StoryPointPlaybook.Api.Hubs;

public class GameHub : Hub
{
    private readonly ILogger<GameHub> _logger;
    private readonly IConnectedUserTracker _tracker;

    public GameHub(ILogger<GameHub> logger, IConnectedUserTracker tracker)
    {
        _logger = logger;
        _tracker = tracker;
    }

    public async Task JoinRoom(string roomCode, Guid roomId, string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);
        _tracker.AddUser(Context.ConnectionId, roomId);

        _logger.LogInformation("Usuário {UserId} entrou na sala {RoomCode}", userId, roomCode);

        await Clients.Group(roomCode).SendAsync("UserJoined", userId);

        var count = _tracker.GetParticipantCount(roomId);
        await Clients.Group(roomCode).SendAsync("ParticipantCountUpdated", count);
    }

    public async Task LeaveRoom(string roomCode, Guid roomId, string userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomCode);
        _tracker.RemoveUser(Context.ConnectionId);

        _logger.LogInformation("Usuário {UserId} saiu da sala {RoomCode}", userId, roomCode);

        await Clients.Group(roomCode).SendAsync("UserLeft", userId);

        var count = _tracker.GetParticipantCount(roomId);
        await Clients.Group(roomCode).SendAsync("ParticipantCountUpdated", count);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _tracker.RemoveUser(Context.ConnectionId);
        _logger.LogInformation("Conexão encerrada: {ConnectionId}", Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }
}
