using Moq;
using FluentAssertions;
using StoryPointPlaybook.Domain.Enums;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Application.CQRS.Handlers;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;
using StoryPointPlaybook.Domain.Exceptions;

namespace StoryPointPlaybook.Tests.Unit.Application;

public class AddStoryHandlerTests
{
    private readonly Mock<IStoryRepository> _storyRepoMock = new();
    private readonly Mock<IRoomRepository> _roomRepoMock = new();
    private readonly Mock<IGameHubNotifier> _hubMock = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly AddStoryHandler _handler;

    public AddStoryHandlerTests() => _handler = new AddStoryHandler(_storyRepoMock.Object, _roomRepoMock.Object, _hubMock.Object, _uowMock.Object);

    [Fact]
    public async Task Handle_RoomNotFound_ThrowsException()
    {
        // Arrange
        _roomRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Room?)null);
        var command = new AddStoryCommand(Guid.NewGuid(), "title", "desc");

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<RoomNotFoundException>();
    }

    [Fact]
    public async Task Handle_ValidRequest_AddsStoryAndNotifies()
    {
        // Arrange
        var room = new Room("Room", VotingScale.Fibonacci, 60, false);
        _roomRepoMock.Setup(r => r.GetByIdAsync(room.Id)).ReturnsAsync(room);
        _storyRepoMock.Setup(r => r.AddAsync(It.IsAny<Story>())).Returns(Task.CompletedTask);
        _hubMock.Setup(h => h.NotifyStoryAdded(room.Code, It.IsAny<StoryResponse>())).Returns(Task.CompletedTask);
        var command = new AddStoryCommand(room.Id, "title", "desc");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Title.Should().Be("title");
        result.Description.Should().Be("desc");
        _storyRepoMock.Verify(s => s.AddAsync(It.IsAny<Story>()), Times.Once);
        _hubMock.Verify(h => h.NotifyStoryAdded(room.Code, It.IsAny<StoryResponse>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

