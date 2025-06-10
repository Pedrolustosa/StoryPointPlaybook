using MediatR;

namespace StoryPointPlaybook.Application.CQRS.Commands;

public record SendMessageCommand(Guid RoomId, string UserName, string Message) : IRequest<Unit>;
