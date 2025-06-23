using MediatR;
using StoryPointPlaybook.Domain.Entities;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;
using StoryPointPlaybook.Application.Events;
using StoryPointPlaybook.Domain.Exceptions;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class AddStoryHandler(
    IUnitOfWork unitOfWork,
    IMediator mediator) : IRequestHandler<AddStoryCommand, StoryResponse>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMediator _mediator = mediator;

    public async Task<StoryResponse> Handle(AddStoryCommand request, CancellationToken cancellationToken)
    {
        var story = new Story(request.Title, request.Description, request.RoomId);
        await _unitOfWork.Stories.AddAsync(story);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new StoryResponse
        {
            Id = story.Id,
            Title = story.Title,
            Description = story.Description
        };

        var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId)
            ?? throw new RoomNotFoundException();
        await _mediator.Publish(new StoryAddedEvent(room.Code, response), cancellationToken);
        return response;
    }
}