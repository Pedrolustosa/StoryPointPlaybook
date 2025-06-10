namespace StoryPointPlaybook.Application.DTOs;

public class ExportResultDto
{
    public string RoomName { get; set; }
    public string RoomCode { get; set; }
    public List<ExportedStoryDto> Stories { get; set; }
}
