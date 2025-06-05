using MediatR;
using StoryPointPlaybook.Application.DTOs;

namespace StoryPointPlaybook.Application.CQRS.Stories.Commands;

public record AddStoryCommand(Guid RoomId, string Title, string Description) : IRequest<StoryDto>;
