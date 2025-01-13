using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Web.Middlewares
{
    public class ExceptionHandlingMiddleware(
        RequestDelegate _next, 
        ILogger<ExceptionHandlingMiddleware> _logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }
        
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, title) = exception switch
            {
                ArgumentException or InvalidOperationException 
                    => (HttpStatusCode.BadRequest, "Invalid request"),
                UnauthorizedAccessException 
                    => (HttpStatusCode.Unauthorized, "Unauthorized access"),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
            };

            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = context.Request.Path
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(problemDetails);
            return context.Response.WriteAsync(json);
        }
    }
}
