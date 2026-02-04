using System.Net;
using System.Text.Json;
using TaskManagement.API.Common.Exceptions;
using TaskManagement.API.Common;

namespace TaskManagement.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            var response = new ErrorResponse();

            switch (exception)
            {
                case BadRequestException badRequestEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = badRequestEx.Message;
                    response.Errors = badRequestEx.Errors.Any()
                        ? badRequestEx.Errors
                        : new List<string> { badRequestEx.Message };

                    _logger.LogWarning("Bad Request for {Method} {Path}: {Message}",
                        context.Request.Method,
                        context.Request.Path,
                        response.Message);
                    break;

                case NotFoundException notFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = notFoundEx.Message;

                    _logger.LogWarning("Data not found for {Method} {Path}: {Message}",
                      context.Request.Method,
                      context.Request.Path,
                      notFoundEx.Message);
                    break;

                default:
                    _logger.LogError(exception, "Unhandled exception for {Method} {Path}",
                         context.Request.Method,
                         context.Request.Path);

                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = "An unexpected error occurred";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
