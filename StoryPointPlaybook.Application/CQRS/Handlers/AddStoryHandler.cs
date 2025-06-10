using MediatR;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class AddStoryHandler : IRequestHandler<AddStoryCommand, StoryResponse>
{
    private readonly IStoryRepository _storyRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IGameHubNotifier _hubNotifier;

    public AddStoryHandler(IStoryRepository storyRepository, IRoomRepository roomRepository,IGameHubNotifier hubNotifier)
    {
        _storyRepository = storyRepository;
        _hubNotifier = hubNotifier;
        _roomRepository = roomRepository;
    }

    public async Task<StoryResponse> Handle(AddStoryCommand request, CancellationToken cancellationToken)
    {
        var story = new Story(request.Title, request.Description, request.RoomId);
        await _storyRepository.AddAsync(story);

        var response = new StoryResponse
        {
            Id = story.Id,
            Title = story.Title,
            Description = story.Description
        };

        var room = await _roomRepository.GetByIdAsync(request.RoomId);
        if (room == null)
            throw new Exception("Sala não encontrada");

        await _hubNotifier.NotifyStoryAdded(room.Code, response);
        return response;
    }
}