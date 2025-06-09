using MediatR;

namespace StoryPointPlaybook.Application.CQRS.Commands;

public record SubmitVoteCommand(Guid StoryId, Guid UserId, string Value) : IRequest;
