namespace StoryPointPlaybook.Application.DTOs;

public class ExportedStoryDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<VoteEntryDto> Votes { get; set; }
    public string Average { get; set; }
}
