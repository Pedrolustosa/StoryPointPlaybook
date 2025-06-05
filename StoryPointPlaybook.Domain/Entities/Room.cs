using StoryPointPlaybook.Domain.Enums;

namespace StoryPointPlaybook.Domain.Entities;

public class Room
{
    public Guid Id { get; private set; }
    public string Code { get; private set; }
    public string Name { get; private set; }
    public VotingScale Scale { get; private set; }
    public int TimeLimitInSeconds { get; private set; }
    public bool AutoReveal { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsClosed { get; private set; }

    public ICollection<User> Participants { get; private set; } = new List<User>();
    public ICollection<Story> Stories { get; private set; } = new List<Story>();

    protected Room() { }

    public Room(string name, VotingScale scale, int timeLimitInSeconds, bool autoReveal)
    {
        Id = Guid.NewGuid();
        Code = GenerateRoomCode();
        Name = name;
        Scale = scale;
        TimeLimitInSeconds = timeLimitInSeconds;
        AutoReveal = autoReveal;
        CreatedAt = DateTime.UtcNow;
        IsClosed = false;
    }

    private string GenerateRoomCode()
    {
        return Guid.NewGuid().ToString("N")[..6].ToUpper();
    }

    public void CloseRoom() => IsClosed = true;
}
