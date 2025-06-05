using MediatR;

namespace StoryPointPlaybook.Application.CQRS.Stories.Commands;

public record SubmitVoteCommand(Guid StoryId, Guid UserId, string Value) : IRequest<Unit>;
