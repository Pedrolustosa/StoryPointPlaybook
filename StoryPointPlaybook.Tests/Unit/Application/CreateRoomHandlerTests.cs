using Moq;
using FluentAssertions;
using StoryPointPlaybook.Domain.Enums;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Application.CQRS.Handlers;
using StoryPointPlaybook.Application.CQRS.Rooms.Commands;

namespace StoryPointPlaybook.Tests.Unit.Application;

public class CreateRoomHandlerTests
{
    private readonly Mock<IRoomRepository> _roomRepoMock = new();
    private readonly CreateRoomHandler _handler;

    public CreateRoomHandlerTests() => _handler = new CreateRoomHandler(_roomRepoMock.Object);

    [Fact]
    public async Task Handle_CreatesRoomAndReturnsDto()
    {
        _roomRepoMock.Setup(r => r.AddAsync(It.IsAny<Room>())).Returns(Task.CompletedTask);
        var command = new CreateRoomCommand("Room", VotingScale.Fibonacci, 60, true, "Creator");

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Name.Should().Be("Room");
        result.Scale.Should().Be(VotingScale.Fibonacci);
        result.AutoReveal.Should().BeTrue();
        result.Participants.Should().HaveCount(1);
        _roomRepoMock.Verify(r => r.AddAsync(It.IsAny<Room>()), Times.Once);
    }
}
