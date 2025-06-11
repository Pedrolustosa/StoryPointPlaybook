using FluentAssertions;
using StoryPointPlaybook.Domain.Entities;

namespace StoryPointPlaybook.Tests.Unit.Domain;

public class VoteTests
{
    [Fact]
    public void SetValue_UpdatesValue()
    {
        var vote = new Vote(Guid.NewGuid(), Guid.NewGuid(), "1");

        vote.SetValue("5");

        vote.Value.Should().Be("5");
    }
}
