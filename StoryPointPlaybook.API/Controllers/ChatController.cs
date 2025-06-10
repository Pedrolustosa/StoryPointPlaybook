using MediatR;
using Microsoft.AspNetCore.Mvc;
using StoryPointPlaybook.API.Common;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Application.CQRS.Queries;
using StoryPointPlaybook.Application.CQRS.Commands;

namespace StoryPointPlaybook.API.Controllers;

[ApiController]
[Route("api/rooms/{roomId}/chat")]
public class ChatController(IMediator mediator, ILogger<ChatController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<ChatController> _logger = logger;

    [HttpPost]
    public async Task<IActionResult> SendMessage(Guid roomId, [FromBody] SendMessageRequest request)
        => await ControllerHelper.ExecuteAsync(async () =>
        {
            var command = new SendMessageCommand(roomId, request.UserName, request.Message);
            await _mediator.Send(command);
        }, _logger, this, Messages.Success.MessageSent);

    [HttpGet]
    public async Task<IActionResult> GetMessages(Guid roomId)
        => await ControllerHelper.ExecuteAsync(async () =>
        {
            var result = await _mediator.Send(new GetChatHistoryQuery(roomId));
            return result;
        }, _logger, this);
}
