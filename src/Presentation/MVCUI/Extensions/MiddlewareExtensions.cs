using MVCUI.Middlewares;

namespace MVCUI.Extensions;

internal static class MiddlewareExtensions
{
    internal static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();

        return app;
    }
    internal static IApplicationBuilder UseAuthorizationHeaderInjection(this IApplicationBuilder app)
    {
        app.UseMiddleware<AuthorizationHeaderInjectionMiddleware>();

        return app;
    }
}
