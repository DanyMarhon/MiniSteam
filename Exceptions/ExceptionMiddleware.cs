using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MiniSteam.CustomExceptions;
using Serilog.Context;
using System.Net;
using System.Text.Json;

namespace MiniSteam.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //Info contextual
            using (LogContext.PushProperty("RequestPath", context.Request.Path))
            using (LogContext.PushProperty("RequestMethod", context.Request.Method))
            using (LogContext.PushProperty("User", context.User?.Identity?.Name ?? "Anonymous"))
            using (LogContext.PushProperty("TraceId", context.TraceIdentifier))
            {
                try
                {
                    await _next(context);
                }
                catch (MiniSteamException ex)
                {
                    _logger.LogError(ex, $"MiniSteamException of type {ex.ErrorType}", ex.ErrorType);
                    await HandleMiniSteamExceptionAsync(context, ex);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception");
                    await HandleGenericExceptionAsync(context, ex);
                }
            }
        }

        private static async Task HandleMiniSteamExceptionAsync(HttpContext context, MiniSteamException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.ErrorType switch
            {
                "Validation" => (int)HttpStatusCode.BadRequest,
                "Unauthorized" => (int)HttpStatusCode.Unauthorized,
                "Forbidden" => (int)HttpStatusCode.Forbidden,
                "NotFound" => (int)HttpStatusCode.NotFound,
                "Conflict" => (int)HttpStatusCode.Conflict,
                "Timeout" => (int)HttpStatusCode.RequestTimeout,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new
            {
                error = ex.ErrorType,
                message = ex.Message,
                traceId = context.TraceIdentifier // para correlación en logs
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static async Task HandleGenericExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                error = "UnhandledException",
                message = "An unexpected error occurred.",
                traceId = context.TraceIdentifier
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}


