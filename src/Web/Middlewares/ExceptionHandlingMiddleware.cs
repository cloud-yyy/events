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
            var problemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "An unexpected error occurred!",
                Detail = exception.Message,
                Instance = context.Request.Path
            };
            
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = problemDetails.Status.Value;
            
            var json = JsonSerializer.Serialize(problemDetails);
            
            return context.Response.WriteAsync(json);
        }
    }
}
