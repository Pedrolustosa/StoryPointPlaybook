namespace StoryPointPlaybook.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Role { get; private set; }
    public Guid RoomId { get; private set; }
    public Room Room { get; private set; }
    public ICollection<Vote> Votes { get; set; } = new List<Vote>();

    protected User() { }

    public User(string name, string role, Guid roomId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Role = role;
        RoomId = roomId;
    }
}

