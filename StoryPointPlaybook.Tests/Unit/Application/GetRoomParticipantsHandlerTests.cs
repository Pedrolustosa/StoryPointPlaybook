using Moq;
using FluentAssertions;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Application.CQRS.Handlers;
using StoryPointPlaybook.Application.CQRS.Queries;

namespace StoryPointPlaybook.Tests.Unit.Application;

public class GetRoomParticipantsHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly GetRoomParticipantsHandler _handler;

    public GetRoomParticipantsHandlerTests()
    {
        _uowMock.Setup(u => u.Users).Returns(_userRepoMock.Object);
        _handler = new GetRoomParticipantsHandler(_uowMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsUserResponses()
    {
        var roomId = Guid.NewGuid();
        var users = new List<User>
        {
            new("Alice", "Member", roomId),
            new("Bob", "Member", roomId)
        };
        _userRepoMock.Setup(r => r.GetByRoomIdAsync(roomId)).ReturnsAsync(users);

        var result = await _handler.Handle(new GetRoomParticipantsQuery(roomId), CancellationToken.None);

        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Alice");
        result[1].Name.Should().Be("Bob");
    }
}
