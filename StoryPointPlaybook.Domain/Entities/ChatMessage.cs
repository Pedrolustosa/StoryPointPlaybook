namespace StoryPointPlaybook.Domain.Entities;

public class ChatMessage
{
    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string User { get; private set; }
    public string Message { get; private set; }
    public DateTime Timestamp { get; private set; }

    protected ChatMessage() { }

    public ChatMessage(Guid roomId, string user, string message)
    {
        Id = Guid.NewGuid();
        RoomId = roomId;
        User = user;
        Message = message;
        Timestamp = DateTime.UtcNow;
    }
}
