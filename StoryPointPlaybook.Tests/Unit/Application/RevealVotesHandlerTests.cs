using Moq;
using FluentAssertions;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Enums;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Application.CQRS.Handlers;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;

namespace StoryPointPlaybook.Tests.Unit.Application;

public class RevealVotesHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<IStoryRepository> _storyRepoMock = new();
    private readonly Mock<IGameHubNotifier> _notifierMock = new();
    private readonly RevealVotesHandler _handler;

    public RevealVotesHandlerTests()
    {
        _uowMock.Setup(u => u.Stories).Returns(_storyRepoMock.Object);
        _handler = new RevealVotesHandler(_notifierMock.Object, _uowMock.Object);
    }

    [Fact]
    public async Task Handle_StoryNotFound_Throws()
    {
        _storyRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Story?)null);
        var act = () => _handler.Handle(new RevealVotesCommand(Guid.NewGuid()), CancellationToken.None);
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Handle_RevealsVotesAndNotifies()
    {
        var story = new Story("title", "desc", Guid.NewGuid());
        _storyRepoMock.Setup(r => r.GetByIdAsync(story.Id)).ReturnsAsync(story);
        _storyRepoMock.Setup(r => r.UpdateAsync(story)).Returns(Task.CompletedTask);
        _notifierMock.Setup(n => n.NotifyVotesRevealed(story.RoomId)).Returns(Task.CompletedTask);

        await _handler.Handle(new RevealVotesCommand(story.Id), CancellationToken.None);

        story.VotesRevealed.Should().BeTrue();
        _storyRepoMock.Verify(r => r.UpdateAsync(story), Times.Once);
        _notifierMock.Verify(n => n.NotifyVotesRevealed(story.RoomId), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }
}