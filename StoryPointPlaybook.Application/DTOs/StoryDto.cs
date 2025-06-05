namespace StoryPointPlaybook.Application.DTOs;

public class StoryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid RoomId { get; set; }
}
