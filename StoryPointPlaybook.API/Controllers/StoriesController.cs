using MediatR;
using StoryPointPlaybook.API.Common;
using StoryPointPlaybook.Application.Common;
using StoryPointPlaybook.Application.CQRS.Commands;
using StoryPointPlaybook.Application.CQRS.Queries;
using StoryPointPlaybook.Application.CQRS.Stories.Commands;
using StoryPointPlaybook.Application.DTOs;

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

    [HttpGet("{storyId}/votes")]
    public async Task<IActionResult> GetRevealedVotes(Guid roomId, Guid storyId)
    => await ControllerHelper.ExecuteAsync(async () =>
    {
        var story = await _mediator.Send(new GetStoryWithVotesQuery(storyId));
        if (story == null)
            throw new Exception(ApplicationErrors.StoryNotFound);

        var revealedVotes = story.Votes
            .Where(v => v.IsRevealed)
            .Select(v => new VoteResultDto
            {
                UserName = v.User.Name,
                Value = v.Value,
                IsRevealed = true
            });

        return revealedVotes;
    }, _logger, this, Messages.Success.VotesRevealed);

    [HttpGet("{storyId}/voting-status")]
    public async Task<IActionResult> GetVotingStatus(Guid roomId, Guid storyId)
    => await ControllerHelper.ExecuteAsync(async () =>
    {
        var statusList = await _mediator.Send(new GetVotingStatusQuery(storyId));
        return statusList;
    }, _logger, this, Messages.Success.VotingStatusRetrieved);


}