using Moq;
using FluentAssertions;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Application.CQRS.Handlers;
using StoryPointPlaybook.Application.CQRS.Queries;

namespace StoryPointPlaybook.Tests.Unit.Application;

public class GetChatHistoryHandlerTests
{
    private readonly Mock<IChatMessageRepository> _repoMock = new();
    private readonly GetChatHistoryHandler _handler;

    public GetChatHistoryHandlerTests() => _handler = new GetChatHistoryHandler(_repoMock.Object);

    [Fact]
    public async Task Handle_ReturnsMappedMessages()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;
        var messages = new List<ChatMessage>
        {
            new(roomId, "User1", "Hi") { },
            new(roomId, "User2", "Hey") { }
        };
        // Set timestamps to fixed values for determinism
        typeof(ChatMessage).GetProperty("Timestamp")!.SetValue(messages[0], timestamp);
        typeof(ChatMessage).GetProperty("Timestamp")!.SetValue(messages[1], timestamp.AddMinutes(1));
        _repoMock.Setup(r => r.GetByRoomIdAsync(roomId)).ReturnsAsync(messages);

        // Act
        var result = await _handler.Handle(new GetChatHistoryQuery(roomId), CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result[0].User.Should().Be("User1");
        result[0].Message.Should().Be("Hi");
        result[0].Timestamp.Should().Be(timestamp.ToString("HH:mm:ss"));
    }
}
