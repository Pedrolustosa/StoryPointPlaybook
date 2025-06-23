using Moq;
using FluentAssertions;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Enums;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.CQRS.Handlers;
using StoryPointPlaybook.Application.CQRS.Queries;

namespace StoryPointPlaybook.Tests.Unit.Application;

public class GetRoomStatisticsHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<IRoomRepository> _roomRepoMock = new();
    private readonly GetRoomStatisticsHandler _handler;

    public GetRoomStatisticsHandlerTests()
    {
        _uowMock.Setup(u => u.Rooms).Returns(_roomRepoMock.Object);
        _handler = new GetRoomStatisticsHandler(_uowMock.Object);
    }

    [Fact]
    public async Task Handle_RoomNotFound_Throws()
    {
        _roomRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Room?)null);
        var act = () => _handler.Handle(new GetRoomStatisticsQuery(Guid.NewGuid()), CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_ComputesStatistics()
    {
        var room = new Room("Room", VotingScale.Fibonacci, 60, false);
        var story1 = new Story("Title1", "Desc1", room.Id);
        var story2 = new Story("Title2", "Desc2", room.Id);
        var user1 = new User("U1", "Member", room.Id);
        var user2 = new User("U2", "Member", room.Id);
        story1.Votes.Add(new Vote(story1.Id, user1.Id, "1"));
        story1.Votes.Add(new Vote(story1.Id, user2.Id, "1"));
        story2.Votes.Add(new Vote(story2.Id, user1.Id, "3"));
        room.Stories.Add(story1);
        room.Stories.Add(story2);
        _roomRepoMock.Setup(r => r.GetByIdAsync(room.Id)).ReturnsAsync(room);

        var result = await _handler.Handle(new GetRoomStatisticsQuery(room.Id), CancellationToken.None);

        result.TotalStories.Should().Be(2);
        result.TotalVotes.Should().Be(3);
        result.DistinctUsers.Should().Be(2);
        result.AvgVotesPerStory.Should().Be(1.5);
        result.ConsensusRate.Should().BeApproximately(50, 0.1);
    }
}
