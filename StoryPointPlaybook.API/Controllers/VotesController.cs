using MediatR;
using Microsoft.AspNetCore.Mvc;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;

namespace StoryPointPlaybook.API.Controllers
{
    [ApiController]
    [Route("api/stories/{storyId}")]
    public class VotesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VotesController(IMediator mediator) => _mediator = mediator;

        [HttpPost("votes")]
        public async Task<IActionResult> SubmitVote(Guid storyId, [FromBody] SubmitVoteCommand command)
        {
            var cmd = command with { StoryId = storyId };
            await _mediator.Send(cmd);
            return Ok();
        }

        [HttpPost("reveal")]
        public async Task<IActionResult> Reveal(Guid storyId)
        {
            await _mediator.Send(new RevealVotesCommand(storyId));
            return Ok();
        }
    }

}
