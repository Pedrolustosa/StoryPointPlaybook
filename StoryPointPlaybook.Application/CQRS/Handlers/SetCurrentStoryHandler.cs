using MediatR;
using StoryPointPlaybook.Application.Common;
using StoryPointPlaybook.Application.CQRS.Commands;
using StoryPointPlaybook.Application.CQRS.Rooms.Commands;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Domain.Interfaces;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class SetCurrentStoryHandler : IRequestHandler<SetCurrentStoryCommand, StoryResponse>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IGameHubNotifier _hubNotifier;

    public SetCurrentStoryHandler(IRoomRepository roomRepository, IGameHubNotifier hubNotifier)
    {
        _roomRepository = roomRepository;
        _hubNotifier = hubNotifier;
    }

    public async Task<StoryResponse> Handle(SetCurrentStoryCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(request.RoomId);
        if (room == null)
            throw new Exception(ApplicationErrors.RoomNotFound);

        var story = room.Stories.FirstOrDefault(s => s.Id == request.StoryId);
        if (story == null)
            throw new Exception(ApplicationErrors.StoryNotFound);

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
