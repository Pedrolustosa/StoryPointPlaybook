namespace StoryPointPlaybook.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; }
    public string Role { get; set; }
    public Guid RoomId { get; set; }
}
