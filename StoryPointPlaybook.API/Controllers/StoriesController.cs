using MediatR;
using Microsoft.AspNetCore.Mvc;
using StoryPointPlaybook.API.Common;
using StoryPointPlaybook.Application.CQRS.Commands;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;

namespace StoryPointPlaybook.API.Controllers;

[ApiController]
[Route("api/rooms/{roomId}/stories")]
public class StoriesController(IMediator mediator, ILogger<StoriesController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<StoriesController> _logger = logger;

    [HttpPost("{storyId}/select")]
    public async Task<IActionResult> SelectStoryForVoting(Guid roomId, Guid storyId)
    => await ControllerHelper.ExecuteAsync(async () =>
    {
        var result = await _mediator.Send(new SetCurrentStoryCommand(roomId, storyId));
        return result;
    }, _logger, this, Messages.Success.CurrentStorySelected);

    [HttpPost]
    public async Task<IActionResult> AddStory(Guid roomId, [FromBody] AddStoryCommand command)
        => await ControllerHelper.ExecuteAsync(async () =>
        {
            var cmd = command with { RoomId = roomId };
            var result = await _mediator.Send(cmd);
            return result;
        }, _logger, this, Messages.Success.StoryAdded);
}