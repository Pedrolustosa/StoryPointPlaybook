using MediatR;

namespace StoryPointPlaybook.Application.Events;

public record VoteSubmittedEvent(Guid RoomId, Guid UserId, bool VotesRevealed) : INotification;
