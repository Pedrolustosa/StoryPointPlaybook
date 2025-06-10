using MediatR;
using StoryPointPlaybook.Application.DTOs;

namespace StoryPointPlaybook.Application.CQRS.Queries;

public record GetChatHistoryQuery(Guid RoomId) : IRequest<List<ChatMessageDto>>;
