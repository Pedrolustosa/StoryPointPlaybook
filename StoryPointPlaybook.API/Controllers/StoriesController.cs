using MediatR;
using Microsoft.AspNetCore.Mvc;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;

namespace StoryPointPlaybook.API.Controllers
{
    [ApiController]
    [Route("api/rooms/{roomId}/stories")]
    public class StoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StoriesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> AddStory(Guid roomId, [FromBody] AddStoryCommand command)
        {
            var cmd = command with { RoomId = roomId };
            var result = await _mediator.Send(cmd);
            return Ok(result);
        }
    }

}
