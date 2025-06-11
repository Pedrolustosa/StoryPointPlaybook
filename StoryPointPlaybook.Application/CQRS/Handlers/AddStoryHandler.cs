using MediatR;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class AddStoryHandler(IStoryRepository storyRepository, IRoomRepository roomRepository, IGameHubNotifier hubNotifier, IUnitOfWork unitOfWork) : IRequestHandler<AddStoryCommand, StoryResponse>
{
    private readonly IStoryRepository _storyRepository = storyRepository;
    private readonly IRoomRepository _roomRepository = roomRepository;
    private readonly IGameHubNotifier _hubNotifier = hubNotifier;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<StoryResponse> Handle(AddStoryCommand request, CancellationToken cancellationToken)
    {
        var story = new Story(request.Title, request.Description, request.RoomId);
        await _storyRepository.AddAsync(story);
        await _unitOfWork.SaveChangesAsync();

        var response = new StoryResponse
        {
            Id = story.Id,
            Title = story.Title,
            Description = story.Description
        };

        var room = await _roomRepository.GetByIdAsync(request.RoomId)??throw new Exception("Sala não encontrada");
        await _hubNotifier.NotifyStoryAdded(room.Code, response);
        return response;
    }
}
