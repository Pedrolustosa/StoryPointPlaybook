using MediatR;

namespace StoryPointPlaybook.Application.CQRS.Stories.Commands;

public record RevealVotesCommand(Guid StoryId) : IRequest<Unit>;
