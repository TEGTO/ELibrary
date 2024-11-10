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

var mergedPath = "merged.json";

if (builder.Environment.IsDevelopment())
{
    Utility.MergeJsonFiles(
        [
        "dev.ocelot.json",
        "dev.ocelot.authentication.json",
        "dev.ocelot.user.json",
        "dev.ocelot.author.json",
        "dev.ocelot.book.json",
        "dev.ocelot.genre.json",
        "dev.ocelot.publisher.json",
        "dev.ocelot.advisor.json",
        "dev.ocelot.cart.json",
        "dev.ocelot.client.json",
        "dev.ocelot.order.json",
        "dev.ocelot.statistics.json",
        "dev.ocelot.stockbook.json",
        "dev.ocelot.chatbot.json",
    ], mergedPath);
}
else
{
    Utility.MergeJsonFiles(
       [
       "ocelot.json",
        "ocelot.authentication.json",
        "ocelot.user.json",
        "ocelot.author.json",
        "ocelot.book.json",
        "ocelot.genre.json",
        "ocelot.publisher.json",
        "ocelot.advisor.json",
        "ocelot.cart.json",
        "ocelot.client.json",
        "ocelot.order.json",
        "ocelot.statistics.json",
        "ocelot.stockbook.json",
        "ocelot.chatbot.json",
    ], mergedPath);
}

builder.Configuration
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile(mergedPath, optional: false, reloadOnChange: true)
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