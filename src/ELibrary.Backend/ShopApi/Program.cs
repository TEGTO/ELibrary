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

var builder = WebApplication.CreateBuilder(args);

#region Cors

bool.TryParse(builder.Configuration[Configuration.USE_CORS], out bool useCors);
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

if (useCors)
{
    builder.Services.AddApplicationCors(builder.Configuration, MyAllowSpecificOrigins, builder.Environment.IsDevelopment());
}

#endregion

builder.Services.AddDbContextFactory<LibraryShopDbContext>(builder.Configuration.GetConnectionString(Configuration.SHOP_DATABASE_CONNECTION_STRING)!);


#region Identity & Authentication

builder.Services.ConfigureIdentityServices(builder.Configuration);

#endregion

#region Project Services 

builder.Services.AddSingleton<IClientService, ClientService>();
builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddSingleton<ICartService, CartService>();
builder.Services.AddSingleton<IOrderManager, OrderManager>();
builder.Services.AddSingleton<IClientManager, ClientManager>();
builder.Services.AddSingleton<IStockBookOrderService, StockBookOrderService>();
builder.Services.AddSingleton<IEventHandler<BookStockAmountUpdatedEvent>, BookStockAmountUpdatedEventHandler>();
builder.Services.AddSingleton<IEventDispatcher, EventDispatcher>();
builder.Services.AddSingleton<IStatisticsService, StatisticsService>();
builder.Services.AddSingleton<IAdvisorService, AdvisorService>();

builder.Services.AddPaginationConfiguration(builder.Configuration);
builder.Services.AddRepositoryPatternWithResilience<LibraryShopDbContext>(builder.Configuration);
builder.Services.AddDefaultResiliencePipeline(builder.Configuration, Configuration.DEFAULT_RESILIENCE_PIPELINE);
builder.Services.AddCustomHttpClientServiceWithResilience(builder.Configuration);
#endregion

builder.Services.AddMemoryCache();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddSharedFluentValidation(typeof(Program));

builder.Services.ConfigureCustomInvalidModelStateResponseControllers();

var app = builder.Build();

if (useCors)
{
    app.UseCors(MyAllowSpecificOrigins);
}

app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
