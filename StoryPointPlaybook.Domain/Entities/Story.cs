namespace StoryPointPlaybook.Domain.Entities;

public class Story
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Guid RoomId { get; private set; }
    public Room Room { get; private set; }
    public bool VotesRevealed { get; private set; }

    public ICollection<Vote> Votes { get; private set; } = new List<Vote>();

    protected Story() { }

    public Story(string title, string description, Guid roomId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        RoomId = roomId;
        VotesRevealed = false;
    }

    public void RevealVotes() => VotesRevealed = true;
    public void ResetVotes()
    {
        Votes.Clear();
        VotesRevealed = false;
    }
}

