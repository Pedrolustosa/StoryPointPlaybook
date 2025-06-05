namespace StoryPointPlaybook.Domain.Entities;

public class Session
{
    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public Room Room { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? EndedAt { get; private set; }

    public Session(Guid roomId)
    {
        Id = Guid.NewGuid();
        RoomId = roomId;
        StartedAt = DateTime.UtcNow;
    }

    public void EndSession() => EndedAt = DateTime.UtcNow;
}

