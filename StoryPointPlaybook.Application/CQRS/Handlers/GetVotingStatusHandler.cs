using MediatR;
using StoryPointPlaybook.Application.CQRS.Queries;
using StoryPointPlaybook.Application.DTOs;
using StoryPointPlaybook.Domain.Exceptions;
using StoryPointPlaybook.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.CQRS.Handlers
{
    public class GetVotingStatusHandler(
        IStoryRepository storyRepo) : IRequestHandler<GetVotingStatusQuery, List<VotingStatusDto>>
    {
        private readonly IStoryRepository _storyRepo = storyRepo;

        public async Task<List<VotingStatusDto>> Handle(GetVotingStatusQuery request, CancellationToken cancellationToken)
        {
            var story = await _storyRepo.GetByIdWithVotesAsync(request.StoryId)
                ?? throw new StoryNotFoundException();

            var voters = story.Room.Participants
                .Where(p => !string.Equals(p.Role, "PO", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var statusList = voters.Select(p => new VotingStatusDto
            {
                UserId = p.Id,
                DisplayName = p.Name,
                HasVoted = story.Votes.Any(v => v.UserId == p.Id && !string.IsNullOrWhiteSpace(v.Value))
            }).ToList();

            return statusList;
        }
    }

}
