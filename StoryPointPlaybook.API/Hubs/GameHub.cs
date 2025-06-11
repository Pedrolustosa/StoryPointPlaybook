using Microsoft.AspNetCore.SignalR;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.Interfaces;

namespace StoryPointPlaybook.API.Hubs;

public class GameHub(ILogger<GameHub> logger, IConnectedUserTracker tracker, IStoryRepository storyRepository) : Hub
{
    private readonly ILogger<GameHub> _logger = logger;
    private readonly IConnectedUserTracker _tracker = tracker;
    private readonly IStoryRepository _storyRepository = storyRepository;

    public async Task JoinRoom(string roomCode, Guid roomId, string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);
        _tracker.AddUser(Context.ConnectionId, roomId);

        _logger.LogInformation("Usuário {UserId} entrou na sala {RoomCode}", userId, roomCode);

        await Clients.Group(roomCode).SendAsync("UserJoined", userId);

        var count = _tracker.GetParticipantCount(roomId);
        await Clients.Group(roomCode).SendAsync("ParticipantCountUpdated", count);

        var stories = await _storyRepository.GetByRoomIdAsync(roomId);
        var storyDtos = stories.Select(s => new StoryResponse
        {
            Id = s.Id,
            Title = s.Title,
            Description = s.Description
        });
        await Clients.Caller.SendAsync("StoriesInitialized", storyDtos);
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
