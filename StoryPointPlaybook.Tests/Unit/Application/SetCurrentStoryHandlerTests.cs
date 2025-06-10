using Moq;
using FluentAssertions;
using StoryPointPlaybook.Domain.Enums;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.Common;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Application.CQRS.Handlers;
using StoryPointPlaybook.Application.CQRS.Commands;

namespace StoryPointPlaybook.Tests.Unit.Application;

public class SetCurrentStoryHandlerTests
{
    private readonly Mock<IRoomRepository> _roomRepoMock = new();
    private readonly Mock<IGameHubNotifier> _hubMock = new();
    private readonly SetCurrentStoryHandler _handler;

    public SetCurrentStoryHandlerTests() => _handler = new SetCurrentStoryHandler(_roomRepoMock.Object, _hubMock.Object);

    [Fact]
    public async Task Handle_RoomNotFound_ThrowsException()
    {
        _roomRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Room?)null);
        var command = new SetCurrentStoryCommand(Guid.NewGuid(), Guid.NewGuid());
        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<Exception>().WithMessage(ApplicationErrors.RoomNotFound);
    }

    [Fact]
    public async Task Handle_StoryNotFound_ThrowsException()
    {
        var room = new Room("Room", VotingScale.Fibonacci, 60, false);
        _roomRepoMock.Setup(r => r.GetByIdAsync(room.Id)).ReturnsAsync(room);
        var command = new SetCurrentStoryCommand(room.Id, Guid.NewGuid());
        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<Exception>().WithMessage(ApplicationErrors.StoryNotFound);
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesRoomAndNotifies()
    {
        var room = new Room("Room", VotingScale.Fibonacci, 60, false);
        var story = new Story("title", "desc", room.Id);
        room.Stories.Add(story);
        _roomRepoMock.Setup(r => r.GetByIdAsync(room.Id)).ReturnsAsync(room);
        _roomRepoMock.Setup(r => r.UpdateAsync(room)).Returns(Task.CompletedTask);
        _hubMock.Setup(h => h.NotifyCurrentStoryChanged(room.Code, It.IsAny<StoryResponse>())).Returns(Task.CompletedTask);
        var command = new SetCurrentStoryCommand(room.Id, story.Id);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Id.Should().Be(story.Id);
        room.CurrentStoryId.Should().Be(story.Id);
        _roomRepoMock.Verify(r => r.UpdateAsync(room), Times.Once);
        _hubMock.Verify(h => h.NotifyCurrentStoryChanged(room.Code, It.IsAny<StoryResponse>()), Times.Once);
    }
}
