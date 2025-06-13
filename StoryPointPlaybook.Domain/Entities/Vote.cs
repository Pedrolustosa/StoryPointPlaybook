namespace StoryPointPlaybook.Domain.Entities;

public class Vote
{
    public Guid Id { get; private set; }
    public Guid StoryId { get; private set; }
    public Story Story { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    public string Value { get; private set; }
    public bool IsRevealed { get; private set; }

    protected Vote() { }

    public Vote(Guid storyId, Guid userId, string value)
    {
        Id = Guid.NewGuid();
        StoryId = storyId;
        UserId = userId;
        Value = value;
    }

    public void SetValue(string value)
    {
        Value = value;
    }

    public void Reveal()
    {
        IsRevealed = true;
    }
}
