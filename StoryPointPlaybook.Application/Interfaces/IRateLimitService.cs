namespace StoryPointPlaybook.Application.Interfaces;

public interface IRateLimitService
{
    bool CanProceed(string userKey, int limit, TimeSpan window);
}
