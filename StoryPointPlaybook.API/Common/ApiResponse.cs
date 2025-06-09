namespace StoryPointPlaybook.API.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public ApiResponse(bool success, string message, T? data = default, List<string>? errors = null)
    {
        Success = success;
        Message = message;
        Data = data;
        Errors = errors;
    }

    public static ApiResponse<T> SuccessResponse(string message, T data)
        => new(true, message, data);

    public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null)
        => new(false, message, default, errors);
}
