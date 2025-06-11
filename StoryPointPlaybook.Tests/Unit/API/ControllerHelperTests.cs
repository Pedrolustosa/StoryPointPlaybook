using Moq;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StoryPointPlaybook.API.Common;

namespace StoryPointPlaybook.Tests.Unit.API;

public class ControllerHelperTests
{
    private class DummyController : ControllerBase { }

    [Fact]
    public async Task ExecuteAsync_ReturnsValidationErrors()
    {
        var failures = new List<ValidationFailure> { new("Field", "Error") };
        var logger = Mock.Of<ILogger>();
        var controller = new DummyController();

        Func<Task<string>> action = () => throw new ValidationException(failures);

        var result = await ControllerHelper.ExecuteAsync(action, logger, controller);

        var badRequest = result.Should().BeOfType<ObjectResult>().Subject;
        badRequest.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        var response = badRequest.Value.Should().BeAssignableTo<ApiResponse<string>>().Subject;
        response.Errors.Should().Contain("Field: Error");
    }
}
