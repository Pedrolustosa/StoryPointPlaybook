namespace StoryPointPlaybook.Application.DTOs;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Role { get; set; } = null!;
}
