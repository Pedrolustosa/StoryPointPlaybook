using FluentAssertions;
using StoryPointPlaybook.API.SignalR;

namespace StoryPointPlaybook.Tests.Unit.Services;

public class ConnectedUserTrackerTests
{
    [Fact]
    public void AddAndRemoveUser_ManageConnections()
    {
        var tracker = new ConnectedUserTracker();
        var roomId = Guid.NewGuid();

        tracker.AddUser("conn1", roomId);
        tracker.GetParticipantCount(roomId).Should().Be(1);

        tracker.RemoveUser("conn1");
        tracker.GetParticipantCount(roomId).Should().Be(0);
    }

    [Fact]
    public void GetParticipantCount_ReturnsCorrectCount()
    {
        var tracker = new ConnectedUserTracker();
        var room1 = Guid.NewGuid();
        var room2 = Guid.NewGuid();

        tracker.AddUser("a", room1);
        tracker.AddUser("b", room1);
        tracker.AddUser("c", room2);

        tracker.GetParticipantCount(room1).Should().Be(2);
        tracker.GetParticipantCount(room2).Should().Be(1);
    }
}
