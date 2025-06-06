using MediatR;
using Microsoft.AspNetCore.Mvc;
using StoryPointPlaybook.Application.CQRS.Queries;
using StoryPointPlaybook.Application.CQRS.Rooms.Commands;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;

[ApiController]
[Route("api/rooms")]
public class RoomsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoomsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{code}/join")]
    public async Task<IActionResult> Join(string code, [FromBody] JoinRoomCommand body)
    {
        var command = body with { RoomCode = code };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{roomId}/participants")]
    public async Task<IActionResult> GetParticipants(Guid roomId)
    {
        var result = await _mediator.Send(new GetRoomParticipantsQuery(roomId));
        return Ok(result);
    }

}
