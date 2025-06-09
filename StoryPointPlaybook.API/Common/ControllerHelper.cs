using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace StoryPointPlaybook.API.Common;

public static class ControllerHelper
{
    public static async Task<IActionResult> ExecuteAsync<T>(
        Func<Task<T>> action,
        ILogger logger,
        ControllerBase controller,
        string successMessage = "Operação realizada com sucesso.")
    {
        try
        {
            var result = await action();
            var response = ApiResponse<T>.SuccessResponse(successMessage, result);
            return controller.Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao processar requisição.");

            var errors = new List<string> { ex.InnerException?.Message ?? ex.Message };
            var response = ApiResponse<T>.ErrorResponse(ex.Message, errors);
            return controller.BadRequest(response);
        }
    }

    public static async Task<IActionResult> ExecuteAsync(
        Func<Task> action,
        ILogger logger,
        ControllerBase controller,
        string successMessage = "Operação realizada com sucesso.")
    {
        try
        {
            await action();
            var response = ApiResponse<string>.SuccessResponse(successMessage, null);
            return controller.Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao processar requisição.");

            var errors = new List<string> { ex.InnerException?.Message ?? ex.Message };
            var response = ApiResponse<string>.ErrorResponse(ex.Message, errors);
            return controller.BadRequest(response);
        }
    }
}
