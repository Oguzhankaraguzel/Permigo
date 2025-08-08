using Application.Abstractions.Data;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;

namespace Persistence;
public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddServices()
            .AddAuthorizationInternal()
            .AddDatabase(configuration)
            .AddHealthChecks(configuration);

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");

        services.AddDbContextPool<PermigoDbContext>(
            options => options
                .UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default))
                .UseSnakeCaseNamingConvention(),
            2048);

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<PermigoDbContext>());
        services.AddScoped<DataSeeder>();
        return services;
    }
    private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddIdentityCore<AppUser>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
        .AddUserManager<UserManager<AppUser>>()
        .AddRoles<AppRole>()
        .AddRoleManager<RoleManager<AppRole>>()
        .AddEntityFrameworkStores<PermigoDbContext>();

        return services;
    }
    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!);

        return services;
    }
}
