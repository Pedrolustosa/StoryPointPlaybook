using MediatR;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.CQRS.Queries;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class GetChatHistoryHandler(IChatMessageRepository repo) : IRequestHandler<GetChatHistoryQuery, List<ChatMessageDto>>
{
    private readonly IChatMessageRepository _chatMessageRepository = repo;

    public async Task<List<ChatMessageDto>> Handle(GetChatHistoryQuery request, CancellationToken cancellationToken)
    {
        var messages = await _chatMessageRepository.GetByRoomIdAsync(request.RoomId);

        return [.. messages.Select(m => new ChatMessageDto
        {
            User = m.User,
            Message = m.Message,
            Timestamp = m.Timestamp.ToString("HH:mm:ss")
        })];
    }
}
