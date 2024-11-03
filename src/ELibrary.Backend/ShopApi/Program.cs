using Authentication;
using EventSourcing;
using LibraryShopEntities.Data;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Middlewares;
using ShopApi;
using ShopApi.Features.AdvisorFeature.Services;
using ShopApi.Features.CartFeature.Services;
using ShopApi.Features.ClientFeature.Services;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Features.StatisticsFeature.Services;
using ShopApi.Features.StockBookOrderFeature.Models;
using ShopApi.Features.StockBookOrderFeature.Services;
using ShopApi.Services;

var builder = WebApplication.CreateBuilder(args);

#region Cors

bool.TryParse(builder.Configuration[Configuration.USE_CORS], out bool useCors);
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

if (useCors)
{
    builder.Services.AddApplicationCors(builder.Configuration, MyAllowSpecificOrigins, builder.Environment.IsDevelopment());
}

#endregion

builder.Services.AddDbContextFactory<ShopDbContext>(builder.Configuration.GetConnectionString(Configuration.SHOP_DATABASE_CONNECTION_STRING)!, "ShopApi");

builder.Host.SerilogConfiguration();

#region Identity & Authentication

builder.Services.ConfigureIdentityServices(builder.Configuration);

#endregion

#region Project Services 

builder.Services.AddSingleton<IClientService, ClientService>();
builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddSingleton<ICartService, CartService>();
builder.Services.AddSingleton<IStockBookOrderService, StockBookOrderService>();
builder.Services.AddSingleton<IEventHandler<BookStockAmountUpdatedEvent>, BookStockAmountUpdatedEventHandler>();
builder.Services.AddSingleton<IEventDispatcher, EventDispatcher>();
builder.Services.AddSingleton<IStatisticsService, StatisticsService>();
builder.Services.AddSingleton<IAdvisorService, AdvisorService>();
builder.Services.AddSingleton<ILibraryService, LibraryService>();

builder.Services.AddPaginationConfiguration(builder.Configuration);
builder.Services.AddRepositoryPatternWithResilience<ShopDbContext>(builder.Configuration);
builder.Services.AddDefaultResiliencePipeline(builder.Configuration, Configuration.DEFAULT_RESILIENCE_PIPELINE);
builder.Services.AddCustomHttpClientServiceWithResilience(builder.Configuration);
#endregion

builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddMemoryCache();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddSharedFluentValidation(typeof(Program));

builder.Services.ConfigureCustomInvalidModelStateResponseControllers();

var app = builder.Build();

if (app.Configuration[Configuration.EF_CREATE_DATABASE] == "true")
{
    await app.ConfigureDatabaseAsync<ShopDbContext>(CancellationToken.None);
}

if (useCors)
{
    app.UseCors(MyAllowSpecificOrigins);
}

app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseIdentity();

app.MapControllers();

app.Run();

public partial class Program { }