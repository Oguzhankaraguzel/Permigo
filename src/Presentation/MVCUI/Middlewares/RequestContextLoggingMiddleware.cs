using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace MVCUI.Middlewares;

public sealed class RequestContextLoggingMiddleware : IMiddleware
{
    private const string CorrelationIdHeaderName = "Correlation-Id";
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        using (LogContext.PushProperty("CorrelationId", GetCorrelationId(context)))
        {
            return next.Invoke(context);
        }
    }
    private static string GetCorrelationId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(
            CorrelationIdHeaderName,
            out StringValues correlationId);

        return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
    }
}
