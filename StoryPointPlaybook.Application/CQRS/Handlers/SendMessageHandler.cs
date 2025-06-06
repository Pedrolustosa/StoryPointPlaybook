using MediatR;
using StoryPointPlaybook.Application.CQRS.Commands;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.CQRS.Handlers
{
    public class SendMessageHandler : IRequestHandler<SendMessageCommand, Unit>
    {
        private readonly IChatMessageRepository _chatRepo;
        private readonly IChatHubNotifier _chatNotifier;

        public SendMessageHandler(IChatMessageRepository chatRepo, IChatHubNotifier chatNotifier)
        {
            _chatRepo = chatRepo;
            _chatNotifier = chatNotifier;
        }

        public async Task<Unit> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var message = new ChatMessage(request.RoomId, request.UserName, request.Message);

            await _chatRepo.AddAsync(message);

            await _chatNotifier.NotifyMessageSent(message.RoomId, message.User, message.Message, message.Timestamp);

            return Unit.Value;
        }
    }


}
