using MediatR;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.CQRS.Queries;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class GetChatHistoryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetChatHistoryQuery, List<ChatMessageDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<ChatMessageDto>> Handle(GetChatHistoryQuery request, CancellationToken cancellationToken)
    {
        var messages = await _unitOfWork.ChatMessages.GetByRoomIdAsync(request.RoomId);

        return messages.Select(m => new ChatMessageDto
        {
            User = m.User,
            Message = m.Message,
            Timestamp = m.Timestamp.ToString("HH:mm:ss")
        }).ToList();
    }
}
