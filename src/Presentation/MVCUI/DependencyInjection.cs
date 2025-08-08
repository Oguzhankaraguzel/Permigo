using Microsoft.Extensions.DependencyInjection;
using MVCUI.Helpers;
using MVCUI.Middlewares;

namespace MVCUI;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services) 
        => services.AddSession()
                   .AddProblemDetails()
                   .AddSession()
                   .AddMiddleware()
                   .AddExceptionHandler<GlobalExceptionHandler>();

    private static IServiceCollection AddMiddleware(this IServiceCollection services) 
    {
        services.AddScoped<RequestContextLoggingMiddleware>();
        services.AddScoped<AuthorizationHeaderInjectionMiddleware>();
        return services;
    }
}
