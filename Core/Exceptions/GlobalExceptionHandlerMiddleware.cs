using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Serilog;
namespace Core.Exceptions
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                
                LogException(ex);
                var errorResponse = GetErrorResponse(ex);
                await RespondWithErrorAsync(context, errorResponse);
            }
        }

        private void LogException(Exception ex)
        {
            _logger.Error(ex,ex.StackTrace ?? "Stack Trace is null!");
        }

        private BaseErrorResponse GetErrorResponse(Exception ex)
        {
            return ex switch
            {
                BaseException baseException => new BaseErrorResponse(
                    baseException.ErrorCode,
                    baseException.Message,
                    StatusCodes.Status400BadRequest
                ),
                _ => new BaseErrorResponse(
                    ErrorCode.InternalServerError,
                    "An unexpected error occurred.",
                    StatusCodes.Status500InternalServerError
                )
            };
        }

        private async Task RespondWithErrorAsync(HttpContext context, BaseErrorResponse errorResponse)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorResponse.StatusCode;

            string jsonResponse = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
