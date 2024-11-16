using ApiGateway;
using ApiGateway.Middlewares;
using Authentication;
using ExceptionHandling;
using Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddLogging();

#region Cors

bool.TryParse(builder.Configuration[Configuration.USE_CORS], out bool useCors);
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

if (useCors)
{
    builder.Services.AddApplicationCors(builder.Configuration, myAllowSpecificOrigins, builder.Environment.IsDevelopment());
}

#endregion

builder.Services.ConfigureIdentityServices(builder.Configuration);
builder.Services.ConfigureCustomInvalidModelStateResponseControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

#region Ocelot

var env = builder.Environment.EnvironmentName;

var mergedPath = $"merged.{env}.json";

Utility.MergeJsonFiles(
    [
        $"ocelot.{env}.json",
        $"ocelot.{env}.authentication.json",
        $"ocelot.{env}.user.json",
        $"ocelot.{env}.author.json",
        $"ocelot.{env}.book.json",
        $"ocelot.{env}.genre.json",
        $"ocelot.{env}.publisher.json",
        $"ocelot.{env}.advisor.json",
        $"ocelot.{env}.cart.json",
        $"ocelot.{env}.client.json",
        $"ocelot.{env}.order.json",
        $"ocelot.{env}.statistics.json",
        $"ocelot.{env}.stockbook.json",
        $"ocelot.{env}.chatbot.json",
    ], mergedPath);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile(mergedPath, optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration).AddPolly();

#endregion

var app = builder.Build();

if (useCors)
{
    app.UseCors(myAllowSpecificOrigins);
}

app.UseSharedMiddlewares();
app.UseMiddleware<TokenFromQueryMiddleware>();

app.UseRouting();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(_ => { });
app.MapHealthChecks("/health");

await app.UseOcelot();
app.Run();

public partial class Program { }