using FluentAssertions;
using StoryPointPlaybook.Domain.Entities;

namespace StoryPointPlaybook.Tests.Unit.Domain;

public class StoryTests
{
    [Fact]
    public void RevealVotes_SetsFlag()
    {
        var story = new Story("Title", "Desc", Guid.NewGuid());

        story.RevealVotes();

        story.VotesRevealed.Should().BeTrue();
    }

    [Fact]
    public void ResetVotes_ClearsVotesAndFlag()
    {
        var story = new Story("Title", "Desc", Guid.NewGuid());
        story.Votes.Add(new Vote(story.Id, Guid.NewGuid(), "1"));
        story.VotesRevealed = true;

        story.ResetVotes();

        story.Votes.Should().BeEmpty();
        story.VotesRevealed.Should().BeFalse();
    }
}
