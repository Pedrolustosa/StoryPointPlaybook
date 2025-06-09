using MediatR;
using Microsoft.AspNetCore.Mvc;
using StoryPointPlaybook.API.Common;
using StoryPointPlaybook.Application.CQRS.Queries;
using StoryPointPlaybook.Application.CQRS.Rooms.Commands;

namespace StoryPointPlaybook.API.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomsController(IMediator mediator, ILogger<RoomsController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<RoomsController> _logger = logger;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomCommand command)
        => await ControllerHelper.ExecuteAsync(async () =>
        {
            var result = await _mediator.Send(command);
            return result;
        }, _logger, this, Messages.Success.RoomCreated);

    [HttpPost("{code}/join")]
    public async Task<IActionResult> Join(string code, [FromBody] JoinRoomCommand body)
        => await ControllerHelper.ExecuteAsync(async () =>
        {
            var command = body with { RoomCode = code };
            var result = await _mediator.Send(command);
            return result;
        }, _logger, this, Messages.Success.UserJoined);

    [HttpGet("{roomId}/participants")]
    public async Task<IActionResult> GetParticipants(Guid roomId)
        => await ControllerHelper.ExecuteAsync(async () =>
        {
            var result = await _mediator.Send(new GetRoomParticipantsQuery(roomId));
            return result;
        }, _logger, this, Messages.Success.ParticipantsRetrieved);
}
