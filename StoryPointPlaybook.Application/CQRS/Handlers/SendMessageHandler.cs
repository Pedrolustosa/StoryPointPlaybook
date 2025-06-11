using MediatR;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Application.CQRS.Commands;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class SendMessageHandler(IChatMessageRepository chatRepo, IChatHubNotifier chatNotifier, IUnitOfWork unitOfWork) : IRequestHandler<SendMessageCommand, Unit>
{
    private readonly IChatMessageRepository _chatRepo = chatRepo;
    private readonly IChatHubNotifier _chatNotifier = chatNotifier;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new ChatMessage(request.RoomId, request.UserName, request.Message);

        await _chatRepo.AddAsync(message);
        await _unitOfWork.SaveChangesAsync();

        await _chatNotifier.NotifyMessageSent(message.RoomId, message.User, message.Message, message.Timestamp);

        return Unit.Value;
    }
}

