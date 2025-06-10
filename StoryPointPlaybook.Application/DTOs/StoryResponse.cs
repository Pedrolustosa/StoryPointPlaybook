namespace StoryPointPlaybook.Application.DTOs;

public class StoryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Status { get; set; } = null!;
    public List<VoteResponse>? Votes { get; set; }
    public string? Average { get; set; }
}
