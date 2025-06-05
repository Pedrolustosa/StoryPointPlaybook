using MediatR;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Application.Interfaces;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Domain.Interfaces;

public class AddStoryHandler : IRequestHandler<AddStoryCommand, StoryDto>
{
    private readonly IStoryRepository _storyRepo;
    private readonly IGameHubNotifier _notifier;

    public AddStoryHandler(IStoryRepository storyRepo, IGameHubNotifier notifier)
    {
        _storyRepo = storyRepo;
        _notifier = notifier;
    }

    public async Task<StoryDto> Handle(AddStoryCommand request, CancellationToken cancellationToken)
    {
        var story = new Story(request.Title, request.Description, request.RoomId);
        await _storyRepo.AddAsync(story);

        var dto = new StoryDto
        {
            Id = story.Id,
            Title = story.Title,
            Description = story.Description,
            RoomId = story.RoomId
        };

        await _notifier.NotifyStoryAdded(story.RoomId, dto);

        return dto;
    }
}
