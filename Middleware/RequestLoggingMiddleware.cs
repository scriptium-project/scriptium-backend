using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using scriptium_backend_dotnet.Models;
using scriptium_backend_dotnet.DB;

namespace scriptium_backend_dotnet.MiddleWare
{
    public class RequestLoggingMiddleware(RequestDelegate next, ApplicationDBContext db, ILogger<RequestLoggingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));
        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly ILogger<RequestLoggingMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task InvokeAsync(HttpContext context)
        {
            var identifier = context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var endpoint = context.Request.Path.ToString();
            var method = context.Request.Method;

            await _next(context);


            context.Response.OnCompleted(async () =>
            {
                int statusCode = context.Response.StatusCode;

                var requestLog = new RequestLog
                {
                    Identifier = identifier,
                    Endpoint = endpoint,
                    Method = method,
                    StatusCode = statusCode,
                    OccurredAt = DateTime.UtcNow
                };

                //_db.RequestLogs.Add(requestLog);

                try
                {
                    await _db.SaveChangesAsync();
                    _logger.LogInformation($"Request logged successfully: {requestLog}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occurred while saving the request log. Error Details: {ex}");
                }
            });
        }
    }
}
