namespace StoryPointPlaybook.Application.DTOs;

public class StoryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Status { get; set; } = null!; // Ex: "InProgress", "Revealed", etc.
    public List<VoteResponse>? Votes { get; set; }
    public string? Average { get; set; } // Ex: "5", "8", "?" ou "N/A"
}
