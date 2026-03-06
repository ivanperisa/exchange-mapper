using System.Text.Json;
using ExchangeMapper.Application.DTOs;

namespace ExchangeMapper.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, code) = exception switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, "NOT_FOUND"),
            ArgumentException => (StatusCodes.Status400BadRequest, "BAD_REQUEST"),
            InvalidOperationException => (StatusCodes.Status400BadRequest, "INVALID_OPERATION"),
            _ => (StatusCodes.Status500InternalServerError, "INTERNAL_ERROR")
        };

        var response = BaseResponse<object>.Fail(
            code,
            exception.Message,
            new RequestInfo
            {
                Method = context.Request.Method,
                Path = context.Request.Path,
                Timestamp = DateTime.UtcNow.ToString("O")
            });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
