using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;

public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly IChatMessageRepository _chatRepo;
    private readonly IValidator<(string, string)> _validator;
    private readonly IRateLimitService _rateLimit;

    public ChatHub(ILogger<ChatHub> logger, IChatMessageRepository chatRepo, IValidator<(string, string)> validator, IRateLimitService rateLimit)
    {
        _logger = logger;
        _chatRepo = chatRepo;
        _validator = validator;
        _rateLimit = rateLimit;
    }

    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
    }

    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
    }

    public async Task SendMessage(string roomId, string user, string message)
    {
        _logger.LogDebug("Tentativa de mensagem de {User} na sala {RoomId}: \"{Message}\"", user, roomId, message);

        if (!_rateLimit.CanProceed(user, 5, TimeSpan.FromSeconds(10)))
        {
            _logger.LogWarning("Rate limit excedido por {User} na sala {RoomId}", user, roomId);
            throw new HubException("Você está enviando mensagens muito rapidamente.");
        }

        var validation = await _validator.ValidateAsync((user, message));
        if (!validation.IsValid)
        {
            _logger.LogWarning("Mensagem inválida de {User}: {Errors}", user, validation.Errors);
            throw new HubException(validation.Errors.First().ErrorMessage);
        }

        var msg = new ChatMessage(Guid.Parse(roomId), user, message);
        await _chatRepo.AddAsync(msg);

        _logger.LogInformation("Mensagem registrada de {User} na sala {RoomId}: \"{Message}\"", user, roomId, message);

        await Clients.Group(roomId).SendAsync("ChatMessage", user, message, msg.Timestamp.ToString("HH:mm:ss"));
    }

    public override Task OnConnectedAsync()
    {
        _logger.LogInformation("Cliente conectado: {ConnectionId}", Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Cliente desconectado: {ConnectionId}", Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}
