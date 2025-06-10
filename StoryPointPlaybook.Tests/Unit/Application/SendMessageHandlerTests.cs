using Moq;
using FluentAssertions;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Application.CQRS.Handlers;
using StoryPointPlaybook.Application.CQRS.Commands;

namespace StoryPointPlaybook.Tests.Unit.Application;

public class SendMessageHandlerTests
{
    private readonly Mock<IChatMessageRepository> _repoMock = new();
    private readonly Mock<IChatHubNotifier> _notifierMock = new();
    private readonly SendMessageHandler _handler;

    public SendMessageHandlerTests() => _handler = new SendMessageHandler(_repoMock.Object, _notifierMock.Object);

    [Fact]
    public async Task Handle_AddsMessageAndNotifies()
    {
        // Arrange
        var command = new SendMessageCommand(Guid.NewGuid(), "Tester", "Hello");
        _repoMock.Setup(r => r.AddAsync(It.IsAny<ChatMessage>())).Returns(Task.CompletedTask);
        _notifierMock.Setup(n => n.NotifyMessageSent(command.RoomId, command.UserName, command.Message, It.IsAny<DateTime>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repoMock.Verify(r => r.AddAsync(It.Is<ChatMessage>(m => m.RoomId == command.RoomId && m.User == command.UserName && m.Message == command.Message)), Times.Once);
        _notifierMock.Verify(n => n.NotifyMessageSent(command.RoomId, command.UserName, command.Message, It.IsAny<DateTime>()), Times.Once);
    }
}
