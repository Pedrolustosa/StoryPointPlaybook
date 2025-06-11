using FluentAssertions;
using StoryPointPlaybook.Domain.Enums;
using StoryPointPlaybook.Domain.Entities;

namespace StoryPointPlaybook.Tests.Unit.Domain;

public class RoomTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var room = new Room("Test", VotingScale.Fibonacci, 30, true);

        room.Name.Should().Be("Test");
        room.Scale.Should().Be(VotingScale.Fibonacci);
        room.TimeLimitInSeconds.Should().Be(30);
        room.AutoReveal.Should().BeTrue();
        room.Code.Should().HaveLength(6);
        room.IsClosed.Should().BeFalse();
    }

    [Fact]
    public void SetCurrentStory_UpdatesProperty()
    {
        var room = new Room("Test", VotingScale.Fibonacci, 30, false);
        var storyId = Guid.NewGuid();

        room.SetCurrentStory(storyId);

        room.CurrentStoryId.Should().Be(storyId);
    }

    [Fact]
    public void CloseRoom_SetsIsClosed()
    {
        var room = new Room("Test", VotingScale.Fibonacci, 30, false);

        room.CloseRoom();

        room.IsClosed.Should().BeTrue();
    }
}
