using FluentAssertions;
using StoryPointPlaybook.Domain.Entities;

namespace StoryPointPlaybook.Tests.Unit.Domain;

public class SessionTests
{
    [Fact]
    public void Constructor_SetsStartTime()
    {
        var session = new Session(Guid.NewGuid());

        session.StartedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        session.EndedAt.Should().BeNull();
    }

    [Fact]
    public void EndSession_SetsEndTime()
    {
        var session = new Session(Guid.NewGuid());

        session.EndSession();

        session.EndedAt.Should().NotBeNull();
    }
}
