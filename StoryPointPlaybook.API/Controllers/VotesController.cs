using MediatR;
using Microsoft.AspNetCore.Mvc;
using StoryPointPlaybook.API.Common;
using StoryPointPlaybook.Application.CQRS.Commands;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;

namespace StoryPointPlaybook.API.Controllers;

[ApiController]
[Route("api/stories/{storyId}")]
public class VotesController(IMediator mediator, ILogger<VotesController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<VotesController> _logger = logger;

    [HttpPost("votes")]
    public async Task<IActionResult> SubmitVote(Guid storyId, [FromBody] SubmitVoteCommand command)
        => await ControllerHelper.ExecuteAsync(async () =>
        {
            var cmd = command with { StoryId = storyId };
            await _mediator.Send(cmd);
        }, _logger, this, Messages.Success.VoteSubmitted);

    [HttpPost("reveal")]
    public async Task<IActionResult> RevealVotes(Guid storyId)
        => await ControllerHelper.ExecuteAsync(async () =>
        {
            await _mediator.Send(new RevealVotesCommand(storyId));
        }, _logger, this, Messages.Success.VotesRevealed);
}