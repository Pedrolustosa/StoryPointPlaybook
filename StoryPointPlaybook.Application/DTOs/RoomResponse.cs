using StoryPointPlaybook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryPointPlaybook.Application.DTOs
{
    public class RoomResponse
    {
        public Guid Id { get; set; }             // ID interno da sala
        public string Code { get; set; } = null!; // Código público (ex: ABC123)
        public string Name { get; set; } = null!;
        public VotingScale Scale { get; set; }
        public int TimeLimit { get; set; }
        public bool AutoReveal { get; set; }

        public UserResponse CreatedBy { get; set; } = null!;
        public List<UserResponse> Participants { get; set; } = new();
        public StoryResponse? CurrentStory { get; set; }
    }

}
