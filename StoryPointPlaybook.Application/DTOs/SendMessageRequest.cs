namespace StoryPointPlaybook.Application.DTOs;

public class SendMessageRequest
{
    public string UserName { get; set; } = null!;
    public string Message { get; set; } = null!;
}
