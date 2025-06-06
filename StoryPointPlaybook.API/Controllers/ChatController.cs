using MediatR;
using Microsoft.AspNetCore.Mvc;
using StoryPointPlaybook.Application.CQRS.Commands;
using StoryPointPlaybook.Application.CQRS.Queries;
using StoryPointPlaybook.Application.DTOs;

namespace StoryPointPlaybook.API.Controllers;

[ApiController]
[Route("api/rooms/{roomId}/chat")]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromRoute] Guid roomId, [FromBody] SendMessageRequest request)
    {
        var command = new SendMessageCommand(roomId, request.UserName, request.Message);
        await _mediator.Send(command);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetMessages([FromRoute] Guid roomId)
    {
        var result = await _mediator.Send(new GetChatHistoryQuery(roomId));
        return Ok(result);
    }
}
