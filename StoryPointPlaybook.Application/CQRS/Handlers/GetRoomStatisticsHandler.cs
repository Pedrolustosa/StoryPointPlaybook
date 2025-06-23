using MediatR;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using StoryPointPlaybook.Application.CQRS.Queries;

namespace StoryPointPlaybook.Application.CQRS.Handlers;

public class GetRoomStatisticsHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetRoomStatisticsQuery, RoomStatisticsDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<RoomStatisticsDto> Handle(GetRoomStatisticsQuery request, CancellationToken cancellationToken)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId) ?? throw new InvalidOperationException("Sala não encontrada.");
        var stories = room.Stories;
        var totalVotes = stories.Sum(s => s.Votes.Count);
        var totalStories = stories.Count;
        var users = stories.SelectMany(s => s.Votes.Select(v => v.UserId)).Distinct().Count();

        double consensusRate = stories.Count == 0 ? 0 :
            stories.Count(s =>
                s.Votes.Count > 1 &&
                s.Votes.All(v => v.Value == s.Votes.First().Value)
            ) * 100.0 / stories.Count;

        return new RoomStatisticsDto
        {
            TotalStories = totalStories,
            TotalVotes = totalVotes,
            DistinctUsers = users,
            AvgVotesPerStory = totalStories == 0 ? 0 : totalVotes / (double)totalStories,
            ConsensusRate = consensusRate
        };
    }
}
