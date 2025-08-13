using System.Text;
using FluentValidation;
using Newtonsoft.Json;

namespace Web.Middlewares;

public class ExceptionMiddleware() : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
            throw;
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = 0;
        string? message = null;
        var serverMessageError = "Произошла ошибка сервера";

        switch (exception)
        {
            case ValidationException ex:
                statusCode = StatusCodes.Status400BadRequest;
                message = ex.Errors.FirstOrDefault()?.ErrorMessage ?? "Произошла ошибка валидации";
                break;
            case not null:
                statusCode = StatusCodes.Status500InternalServerError;
                message = serverMessageError;
                break;
        }

        var response = new
        {
            statusCode,
            message,
            errorMessage = serverMessageError
        };

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = response.statusCode;

        var jsonMessage = JsonConvert.SerializeObject(response);
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(jsonMessage);
        await httpContext.Response.WriteAsync(jsonMessage);
    }
}