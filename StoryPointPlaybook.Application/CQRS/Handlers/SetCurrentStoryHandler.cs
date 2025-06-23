using MediatR;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Domain.Exceptions;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Application.CQRS.Commands;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class SetCurrentStoryHandler(IGameHubNotifier hubNotifier, IUnitOfWork unitOfWork) : IRequestHandler<SetCurrentStoryCommand, StoryResponse>
{
    private readonly IGameHubNotifier _hubNotifier = hubNotifier;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<StoryResponse> Handle(SetCurrentStoryCommand request, CancellationToken cancellationToken)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId)
            ?? throw new RoomNotFoundException();
        var story = room.Stories.FirstOrDefault(s => s.Id == request.StoryId)
            ?? throw new StoryNotFoundException();
        room.SetCurrentStory(story.Id);
        await _unitOfWork.Rooms.UpdateAsync(room);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var storyDto = new StoryResponse
        {
            Id = story.Id,
            Title = story.Title,
            Description = story.Description
        };

        await _hubNotifier.NotifyCurrentStoryChanged(room.Code, storyDto);
        return storyDto;
    }
}
