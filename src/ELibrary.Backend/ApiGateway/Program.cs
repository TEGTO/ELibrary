using ApiGateway;
using ApiGateway.Middlewares;
using Authentication;
using ExceptionHandling;
using Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddLogging();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

#region Cors

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var allowedOrigins = builder.Configuration.GetSection(Configuration.ALLOWED_CORS_ORIGINS).Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowCredentials()
        .AllowAnyMethod();
        if (builder.Environment.IsDevelopment())
        {
            policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
        }
    });
});

#endregion

builder.Services.ConfigureIdentityServices(builder.Configuration);

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

app.UseCors(MyAllowSpecificOrigins);
app.UseExceptionMiddleware();
app.UseMiddleware<TokenFromQueryMiddleware>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(_ => { });
app.MapHealthChecks("/health");

await app.UseOcelot();
app.Run();
