namespace StoryPointPlaybook.Application.DTOs;

public class VoteResultDto
{
    public string UserName { get; set; } = default!;
    public string Value { get; set; } = default!;
    public bool IsRevealed { get; set; }
}
