using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.DTOs
{
    public class VotingStatusDto
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; } = default!;
        public bool HasVoted { get; set; }
        public string? VoteValue { get; set; }
    }

}
