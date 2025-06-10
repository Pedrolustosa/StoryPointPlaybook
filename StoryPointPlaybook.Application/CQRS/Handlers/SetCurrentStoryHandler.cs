using MediatR;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.Common;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Application.CQRS.Commands;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class SetCurrentStoryHandler(IRoomRepository roomRepository, IGameHubNotifier hubNotifier) : IRequestHandler<SetCurrentStoryCommand, StoryResponse>
{
    private readonly IRoomRepository _roomRepository = roomRepository;
    private readonly IGameHubNotifier _hubNotifier = hubNotifier;

    public async Task<StoryResponse> Handle(SetCurrentStoryCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(request.RoomId)??throw new Exception(ApplicationErrors.RoomNotFound);
        var story = room.Stories.FirstOrDefault(s => s.Id == request.StoryId)??throw new Exception(ApplicationErrors.StoryNotFound);
        room.SetCurrentStory(story.Id);
        await _roomRepository.UpdateAsync(room);

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
