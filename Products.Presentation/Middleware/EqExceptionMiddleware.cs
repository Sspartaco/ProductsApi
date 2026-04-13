using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Products.Presentation.Middleware;

public class EqExceptionMiddleware(RequestDelegate next)
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false
    };

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

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            ArgumentException => (int)HttpStatusCode.BadRequest,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var errorResponse = new
        {
            StatusCode = statusCode,
            exception.Message,
            InnerMessage = exception.InnerException?.Message,
            ExceptionType = exception.GetType().Name,
            StackTrace = statusCode == 500 ? exception.StackTrace : null
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, _jsonOptions));
    }
}
