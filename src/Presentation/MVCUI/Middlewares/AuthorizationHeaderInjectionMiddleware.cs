using Microsoft.AspNetCore.Http;

namespace MVCUI.Middlewares;

public sealed class AuthorizationHeaderInjectionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string? JWToken = context.Session.GetString("Token");
        if (!string.IsNullOrEmpty(JWToken))
        {
            context.Request.Headers["Authorization"] = "Bearer " + JWToken;
        }
        await next(context);
    }
}
