using SafeMedConnect.Domain.Enums;

namespace SafeMedConnect.Domain.Models;

public class ApiResponse
{
    public ApiResponse(ApiResponseTypes apiResponseType)
    {
        ApiResponseType = apiResponseType;
    }

    public ApiResponse(ApiResponseTypes apiResponseType, string message)
    {
        ApiResponseType = apiResponseType;
        Message = message;
    }

    public ApiResponseTypes ApiResponseType { get; }
    public string Message { get; } = string.Empty;
}

public sealed class ApiResponse<T> : ApiResponse
{
    public ApiResponse(ApiResponseTypes apiResponseType) : base(apiResponseType)
    {
    }

    public ApiResponse(ApiResponseTypes apiResponseType, string message) : base(apiResponseType, message)
    {
    }

    public ApiResponse(ApiResponseTypes apiResponseType, T data) : base(apiResponseType)
    {
        Data = data;
    }

    public ApiResponse(ApiResponseTypes apiResponseType, string message, T data) : base(apiResponseType, message)
    {
        Data = data;
    }

    public T? Data { get; }
}