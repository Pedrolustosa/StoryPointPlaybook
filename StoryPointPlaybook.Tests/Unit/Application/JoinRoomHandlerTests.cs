using FluentAssertions;
using Moq;
using StoryPointPlaybook.Application.CQRS.Handlers;
using StoryPointPlaybook.Application.CQRS.Rooms.Commands;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Enums;
using StoryPointPlaybook.Domain.Interfaces;

namespace StoryPointPlaybook.Tests.Unit.Application;

public class JoinRoomHandlerTests
{
    private readonly Mock<IRoomRepository> _roomRepoMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly JoinRoomHandler _handler;

    public JoinRoomHandlerTests()
    {
        _roomRepoMock = new Mock<IRoomRepository>();
        _userRepoMock = new Mock<IUserRepository>();
        _handler = new JoinRoomHandler(_roomRepoMock.Object, _userRepoMock.Object);
    }

    [Fact]
    public async Task Handle_RoomNotFound_ThrowsInvalidOperationException()
    {
        // Arrange
        _roomRepoMock.Setup(r => r.GetByCodeAsync(It.IsAny<string>())).ReturnsAsync((Room?)null);
        var command = new JoinRoomCommand("ABC123", "Tester", "Member");

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_RoomIsClosed_ThrowsInvalidOperationException()
    {
        // Arrange
        var room = new Room("Room", VotingScale.Fibonacci, 60, false);
        room.CloseRoom();
        _roomRepoMock.Setup(r => r.GetByCodeAsync(It.IsAny<string>())).ReturnsAsync(room);
        var command = new JoinRoomCommand("ABC123", "Tester", "Member");

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_ValidRoom_AddsUserAndReturnsDto()
    {
        // Arrange
        var room = new Room("Room", VotingScale.Fibonacci, 60, false);
        _roomRepoMock.Setup(r => r.GetByCodeAsync(It.IsAny<string>())).ReturnsAsync(room);
        _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        var command = new JoinRoomCommand("ABC123", "Tester", "Member");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Tester");
        result.Role.Should().Be("Member");
        result.RoomId.Should().Be(room.Id);
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }
}
