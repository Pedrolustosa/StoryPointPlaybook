using StoryPointPlaybook.Domain.Enums;

namespace StoryPointPlaybook.Application.DTOs;

public class RoomResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public VotingScale Scale { get; set; }
    public int TimeLimit { get; set; }
    public bool AutoReveal { get; set; }

    public UserResponse CreatedBy { get; set; } = null!;
    public List<UserResponse> Participants { get; set; } = new();
    public StoryResponse? CurrentStory { get; set; }
}
