using MediatR;
using StoryPointPlaybook.Application.DTOs;

namespace StoryPointPlaybook.Application.CQRS.Commands;

public record SetCurrentStoryCommand(Guid RoomId, Guid StoryId) : IRequest<StoryResponse>;
