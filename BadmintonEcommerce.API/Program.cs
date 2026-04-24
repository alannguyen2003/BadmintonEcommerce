using System.Reflection;
using BadmintonEcommerce.API;
using BadmintonEcommerce.API.Extensions;
using BadmintonEcommerce.Application;
using BadmintonEcommerce.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

/*
builder.Services.AddSwaggerGenWithAuth();
*/

builder.Services
    .AddInfrastructureLayer(builder.Configuration)
    .AddApplication()
    .AddPresentation();

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

WebApplication app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();

    app.ApplyMigrations();

    app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

/*
app.UseRequestContextLogging();
*/

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();
app.MapEndpoints();


// REMARK: If you want to use Controllers, you'll need this.
app.MapControllers();

await app.RunAsync();

// REMARK: Required for functional and integration tests to work.
namespace Web.Api
{
    public partial class Program;
}