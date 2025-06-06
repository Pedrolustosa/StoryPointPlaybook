using MediatR;
using StoryPointPlaybook.Application.CQRS.Queries;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.CQRS.Handlers
{
    public class GetChatHistoryHandler : IRequestHandler<GetChatHistoryQuery, List<ChatMessageDto>>
    {
        private readonly IChatMessageRepository _repo;

        public GetChatHistoryHandler(IChatMessageRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ChatMessageDto>> Handle(GetChatHistoryQuery request, CancellationToken cancellationToken)
        {
            var messages = await _repo.GetByRoomIdAsync(request.RoomId);

            return messages.Select(m => new ChatMessageDto
            {
                User = m.User,
                Message = m.Message,
                Timestamp = m.Timestamp.ToString("HH:mm:ss")
            }).ToList();
        }
    }

}
