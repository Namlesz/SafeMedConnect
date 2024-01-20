namespace SafeMedConnect.Domain.Responses;

public class ResponseWrapper
{
    public ResponseTypes ResponseType { get; }
    public string Message { get; } = string.Empty;

    public ResponseWrapper(ResponseTypes responseType)
    {
        ResponseType = responseType;
    }

    public ResponseWrapper(ResponseTypes responseType, string message)
    {
        ResponseType = responseType;
        Message = message;
    }
}

public sealed class ResponseWrapper<T> : ResponseWrapper
{
    public T? Data { get; }

    public ResponseWrapper(ResponseTypes responseType) : base(responseType)
    {
    }

    public ResponseWrapper(ResponseTypes responseType, string message) : base(responseType, message)
    {
    }

    public ResponseWrapper(ResponseTypes responseType, T data) : base(responseType)
    {
        Data = data;
    }

    public ResponseWrapper(ResponseTypes responseType, string message, T data) : base(responseType, message)
    {
        Data = data;
    }
}