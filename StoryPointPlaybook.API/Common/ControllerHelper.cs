using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Microsoft.AspNetCore.Http;

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
        catch (ValidationException ve)
        {
            logger.LogWarning(ve, "Falha de validação.");
            var errors = ve.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToList();
            var response = ApiResponse<T>.ErrorResponse("Erro de validação.", errors);
            return controller.BadRequest(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao processar requisição.");
            var errors = new List<string> { ex.InnerException?.Message ?? ex.Message };
            var response = ApiResponse<T>.ErrorResponse("Erro interno do servidor.", errors);
            return controller.StatusCode(StatusCodes.Status500InternalServerError, response);
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
        catch (ValidationException ve)
        {
            logger.LogWarning(ve, "Falha de validação.");
            var errors = ve.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToList();
            var response = ApiResponse<string>.ErrorResponse("Erro de validação.", errors);
            return controller.BadRequest(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao processar requisição.");
            var errors = new List<string> { ex.InnerException?.Message ?? ex.Message };
            var response = ApiResponse<string>.ErrorResponse("Erro interno do servidor.", errors);
            return controller.StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }
}
