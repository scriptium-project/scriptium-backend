using writings_backend_dotnet.Models;
using writings_backend_dotnet.DB;
namespace writings_backend_dotnet.MiddleWare
{

    //TODO: Will be implemented
    public class RequestLoggingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var identifier = context.Session.Id ?? context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var endpoint = context.Request.Path.ToString();
            var method = context.Request.Method;

            await _next(context);

            context.Response.OnCompleted(async () =>
            {
                var statusCode = context.Response.StatusCode;

                var dbContext = context.RequestServices.GetRequiredService<ApplicationDBContext>();

                var requestLog = new RequestLog
                {
                    Identifier = identifier,
                    Endpoint = endpoint,
                    Method = method,
                    StatusCode = statusCode,
                    OccurredAt = DateTime.UtcNow
                };

                //dbContext.RequestLog.Add(requestLog);
                await dbContext.SaveChangesAsync();
            });
        }
    }
}