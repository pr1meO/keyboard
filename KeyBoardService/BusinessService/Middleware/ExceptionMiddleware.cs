using BusinessService.Contracts;
using System.Net;

namespace BusinessService.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                await _next.Invoke(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ExceptionResponse response = exception switch
            {
                ApplicationException _ => new() { StatusCode = HttpStatusCode.BadRequest, Message = "Application exception occurred." },
                KeyNotFoundException _ => new() { StatusCode = HttpStatusCode.NotFound, Message = "The request key not found." },
                UnauthorizedAccessException _ => new() { StatusCode = HttpStatusCode.Unauthorized, Message = "Unauthorized." },
                _ => new() { StatusCode = HttpStatusCode.InternalServerError, Message = "Internal server error." }
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
