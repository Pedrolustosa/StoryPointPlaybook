using MediatR;
using StoryPointPlaybook.Application.CQRS.Queries;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.CQRS.Handlers
{
    public class GetRoomStatisticsHandler : IRequestHandler<GetRoomStatisticsQuery, RoomStatisticsDto>
    {
        private readonly IRoomRepository _roomRepo;

        public GetRoomStatisticsHandler(IRoomRepository roomRepo)
        {
            _roomRepo = roomRepo;
        }

        public async Task<RoomStatisticsDto> Handle(GetRoomStatisticsQuery request, CancellationToken cancellationToken)
        {
            var room = await _roomRepo.GetByIdAsync(request.RoomId);
            if (room == null)
                throw new InvalidOperationException("Sala não encontrada.");

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

}
