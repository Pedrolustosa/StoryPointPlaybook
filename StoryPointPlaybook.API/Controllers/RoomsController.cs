using MediatR;
using Microsoft.AspNetCore.Mvc;
using StoryPointPlaybook.Application.CQRS.Rooms.Commands;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;

[ApiController]
[Route("api/rooms")]
public class RoomsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoomsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{code}/join")]
    public async Task<IActionResult> Join(string code, [FromBody] JoinRoomCommand command)
    {
        var joinCommand = command with { RoomCode = code };
        var result = await _mediator.Send(joinCommand);
        return Ok(result);
    }

    [HttpPost("rooms/{roomId}/stories")]
    public async Task<IActionResult> AddStory(Guid roomId, [FromBody] AddStoryCommand command)
    {
        var newCommand = command with { RoomId = roomId };
        var result = await _mediator.Send(newCommand);
        return Ok(result);
    }

    [HttpPost("stories/{storyId}/votes")]
    public async Task<IActionResult> SubmitVote(Guid storyId, [FromBody] SubmitVoteCommand command)
    {
        var newCommand = command with { StoryId = storyId };
        await _mediator.Send(newCommand);
        return Ok();
    }

    [HttpPost("stories/{storyId}/reveal")]
    public async Task<IActionResult> RevealVotes(Guid storyId)
    {
        await _mediator.Send(new RevealVotesCommand(storyId));
        return Ok();
    }

}
