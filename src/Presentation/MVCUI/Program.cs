using System.Text.Json.Serialization;
using Application;
using HealthChecks.UI.Client;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MVCUI;
using MVCUI.Extensions;
using MVCUI.Helpers;
using MVCUI.Middlewares;
using Persistence;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(o => o.JsonSerializerOptions
           .Converters.Add(new JsonStringEnumConverter()));
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPresentation();

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    await app.ApplyMigrationsAsync();
}
app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseRequestContextLogging();
app.UseSerilogRequestLogging();
app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthorizationHeaderInjection();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


await app.RunAsync();

namespace MVCUI
{
    public partial class Program;
}
