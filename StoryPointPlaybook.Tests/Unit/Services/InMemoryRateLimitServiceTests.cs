using FluentAssertions;
using StoryPointPlaybook.Application.Services;

namespace StoryPointPlaybook.Tests.Unit.Services;

public class InMemoryRateLimitServiceTests
{
    [Fact]
    public void CanProceed_RespectsLimit()
    {
        var svc = new InMemoryRateLimitService();

        for (int i = 0; i < 5; i++)
            svc.CanProceed("user", 5, TimeSpan.FromSeconds(1)).Should().BeTrue();

        svc.CanProceed("user", 5, TimeSpan.FromSeconds(1)).Should().BeFalse();
    }

    [Fact]
    public void CanProceed_AllowsAfterWindow()
    {
        var svc = new InMemoryRateLimitService();

        svc.CanProceed("user", 1, TimeSpan.FromMilliseconds(10)).Should().BeTrue();
        Task.Delay(20).Wait();

        svc.CanProceed("user", 1, TimeSpan.FromMilliseconds(10)).Should().BeTrue();
    }
}
