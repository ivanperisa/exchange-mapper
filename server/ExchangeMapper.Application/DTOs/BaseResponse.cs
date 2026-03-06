namespace ExchangeMapper.Application.DTOs;

public class RequestInfo
{
    public string Method { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Timestamp { get; set; } = string.Empty;
}

public class ErrorDetails
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class BaseResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public ErrorDetails? Error { get; set; }
    public RequestInfo? Request { get; set; }

    public static BaseResponse<T> Ok(T? data, RequestInfo? request = null)
    {
        return new BaseResponse<T>
        {
            Success = true,
            Data = data,
            Error = null,
            Request = request
        };
    }

    public static BaseResponse<T> Fail(string code, string message, RequestInfo? request = null)
    {
        return new BaseResponse<T>
        {
            Success = false,
            Data = default,
            Error = new ErrorDetails
            {
                Code = code,
                Message = message
            },
            Request = request
        };
    }
}
