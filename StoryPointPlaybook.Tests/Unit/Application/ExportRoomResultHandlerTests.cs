using Moq;
using FluentAssertions;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Enums;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.CQRS.Handlers;
using StoryPointPlaybook.Application.CQRS.Queries;

namespace StoryPointPlaybook.Tests.Unit.Application;

public class ExportRoomResultHandlerTests
{
    private readonly Mock<IRoomRepository> _roomRepoMock = new();
    private readonly ExportRoomResultHandler _handler;

    public ExportRoomResultHandlerTests() => _handler = new ExportRoomResultHandler(_roomRepoMock.Object);

    [Fact]
    public async Task Handle_RoomNotFound_Throws()
    {
        _roomRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Room?)null);

        var act = () => _handler.Handle(new ExportRoomResultQuery(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_ReturnsExportedData()
    {
        var room = new Room("Room", VotingScale.Fibonacci, 60, false);
        var user = new User("Tester", "Member", room.Id);
        var story = new Story("Title", "Desc", room.Id);
        var vote = new Vote(story.Id, user.Id, "3");
        typeof(Vote).GetProperty("User")!.SetValue(vote, user);
        story.Votes.Add(vote);
        room.Stories.Add(story);
        _roomRepoMock.Setup(r => r.GetByIdAsync(room.Id)).ReturnsAsync(room);

        var result = await _handler.Handle(new ExportRoomResultQuery(room.Id), CancellationToken.None);

        result.RoomCode.Should().Be(room.Code);
        result.Stories.Should().HaveCount(1);
        result.Stories[0].Average.Should().Be("3.0");
    }
}
