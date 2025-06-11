using Moq;
using FluentAssertions;
using StoryPointPlaybook.Domain.Enums;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Domain.Exceptions;
using MediatR;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Application.CQRS.Handlers;
using StoryPointPlaybook.Application.CQRS.Commands;
using StoryPointPlaybook.Application.Events;

namespace StoryPointPlaybook.Tests.Unit.Application;

public class SubmitVoteHandlerTests
{
    private readonly Mock<IStoryRepository> _storyRepoMock = new();
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<IVoteRepository> _voteRepoMock = new();
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly SubmitVoteHandler _handler;

    public SubmitVoteHandlerTests() => _handler = new SubmitVoteHandler(_storyRepoMock.Object, _userRepoMock.Object, _voteRepoMock.Object, _uowMock.Object, _mediatorMock.Object);

    [Fact]
    public async Task Handle_StoryNotFound_ThrowsException()
    {
        _storyRepoMock.Setup(s => s.GetByIdWithRoomAsync(It.IsAny<Guid>())).ReturnsAsync((Story?)null);
        var command = new SubmitVoteCommand(Guid.NewGuid(), Guid.NewGuid(), "1");
        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<StoryNotFoundException>();
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsException()
    {
        var room = new Room("Room", VotingScale.Fibonacci, 60, false);
        var story = new Story("title", "desc", room.Id);
        typeof(Story).GetProperty("Room")!.SetValue(story, room);
        _storyRepoMock.Setup(s => s.GetByIdWithRoomAsync(story.Id)).ReturnsAsync(story);
        _userRepoMock.Setup(u => u.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);
        var command = new SubmitVoteCommand(story.Id, Guid.NewGuid(), "1");
        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Fact]
    public async Task Handle_NewVote_AddsVoteAndNotifies()
    {
        var room = new Room("Room", VotingScale.Fibonacci, 60, false);
        var story = new Story("title", "desc", room.Id);
        typeof(Story).GetProperty("Room")!.SetValue(story, room);
        var user = new User("Tester", "Member", room.Id);
        room.Participants.Add(user);
        _storyRepoMock.Setup(s => s.GetByIdWithRoomAsync(story.Id)).ReturnsAsync(story);
        _userRepoMock.Setup(u => u.GetByIdAsync(user.Id)).ReturnsAsync(user);
        _voteRepoMock.Setup(v => v.AddAsync(It.IsAny<Vote>()))
            .Callback<Vote>(v => story.Votes.Add(v))
            .Returns(Task.CompletedTask);
        _mediatorMock.Setup(m => m.Publish(It.IsAny<VoteSubmittedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        var command = new SubmitVoteCommand(story.Id, user.Id, "3");

        await _handler.Handle(command, CancellationToken.None);

        _voteRepoMock.Verify(v => v.AddAsync(It.IsAny<Vote>()), Times.Once);
        _mediatorMock.Verify(m => m.Publish(It.IsAny<VoteSubmittedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ExistingVote_UpdatesVote()
    {
        var room = new Room("Room", VotingScale.Fibonacci, 60, false);
        var story = new Story("title", "desc", room.Id);
        typeof(Story).GetProperty("Room")!.SetValue(story, room);
        var user = new User("Tester", "Member", room.Id);
        var existingVote = new Vote(story.Id, user.Id, "2");
        story.Votes.Add(existingVote);
        room.Participants.Add(user);
        _storyRepoMock.Setup(s => s.GetByIdWithRoomAsync(story.Id)).ReturnsAsync(story);
        _userRepoMock.Setup(u => u.GetByIdAsync(user.Id)).ReturnsAsync(user);
        _voteRepoMock.Setup(v => v.UpdateAsync(existingVote)).Returns(Task.CompletedTask);
        _mediatorMock.Setup(m => m.Publish(It.IsAny<VoteSubmittedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        var command = new SubmitVoteCommand(story.Id, user.Id, "5");

        await _handler.Handle(command, CancellationToken.None);

        existingVote.Value.Should().Be("5");
        _voteRepoMock.Verify(v => v.UpdateAsync(existingVote), Times.Once);
        _mediatorMock.Verify(m => m.Publish(It.IsAny<VoteSubmittedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_AutoReveal_RevealsVotesWhenAllUsersVoted()
    {
        var room = new Room("Room", VotingScale.Fibonacci, 60, true);
        var story = new Story("title", "desc", room.Id);
        typeof(Story).GetProperty("Room")!.SetValue(story, room);
        var user1 = new User("User1", "Member", room.Id);
        var user2 = new User("User2", "Member", room.Id);
        room.Participants.Add(user1);
        room.Participants.Add(user2);
        var vote1 = new Vote(story.Id, user1.Id, "1");
        story.Votes.Add(vote1);
        _storyRepoMock.Setup(s => s.GetByIdWithRoomAsync(story.Id)).ReturnsAsync(story);
        _userRepoMock.Setup(u => u.GetByIdAsync(user2.Id)).ReturnsAsync(user2);
        _voteRepoMock.Setup(v => v.AddAsync(It.IsAny<Vote>()))
            .Callback<Vote>(v => story.Votes.Add(v))
            .Returns(Task.CompletedTask);
        _mediatorMock.Setup(m => m.Publish(It.IsAny<VoteSubmittedEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _storyRepoMock.Setup(s => s.UpdateAsync(story)).Returns(Task.CompletedTask);
        var command = new SubmitVoteCommand(story.Id, user2.Id, "2");

        await _handler.Handle(command, CancellationToken.None);

        story.VotesRevealed.Should().BeTrue();
        _mediatorMock.Verify(m => m.Publish(It.IsAny<VoteSubmittedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        _storyRepoMock.Verify(s => s.UpdateAsync(story), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Exactly(2));
    }
}

