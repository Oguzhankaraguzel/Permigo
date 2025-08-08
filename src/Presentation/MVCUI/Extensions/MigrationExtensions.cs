using Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace MVCUI.Extensions;

internal static class MigrationExtensions
{
    /// <summary>
    /// Applies pending EF Core migrations and seeds default data.
    /// Call once at startup (development only or guarded by env check).
    /// </summary>
    internal static async Task ApplyMigrationsAsync(this IApplicationBuilder app)
    {
        await using AsyncServiceScope scope = app.ApplicationServices.CreateAsyncScope();

        PermigoDbContext dbContext = scope.ServiceProvider.GetRequiredService<PermigoDbContext>();
        await dbContext.Database.MigrateAsync();

        DataSeeder seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        await seeder.SeedAsync();
    }
}
