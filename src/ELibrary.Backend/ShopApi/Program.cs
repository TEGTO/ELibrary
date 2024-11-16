using Authentication;
using Caching;
using DatabaseControl;
using EventSourcing;
using ExceptionHandling;
using LibraryShopEntities.Data;
using LibraryShopEntities.Filters;
using LibraryShopEntities.Repositories.Shop;
using Logging;
using Microsoft.EntityFrameworkCore;
using Pagination;
using Resilience;
using Shared;
using ShopApi;
using ShopApi.Features.AdvisorFeature.Domain.Dtos;
using ShopApi.Features.AdvisorFeature.Services;
using ShopApi.Features.CartFeature.Services;
using ShopApi.Features.ClientFeature.Services;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Features.StatisticsFeature.Domain.Dtos;
using ShopApi.Features.StatisticsFeature.Repository;
using ShopApi.Features.StatisticsFeature.Services;
using ShopApi.Features.StockBookOrderFeature.Models;
using ShopApi.Features.StockBookOrderFeature.Services;
using ShopApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<ShopDbContext>(builder.Configuration.GetConnectionString(Configuration.SHOP_DATABASE_CONNECTION_STRING)!, "ShopApi");
builder.Host.AddLogging();

#region Identity & Authentication

builder.Services.ConfigureIdentityServices(builder.Configuration);

#endregion

#region Project Services 

builder.Services.AddSingleton<ICartRepository, CartRepository>();
builder.Services.AddSingleton<IClientRepository, ClientRepository>();
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<IStatisticsRepository, StatisticsRepository>();
builder.Services.AddSingleton<IStockBookOrderRepository, StockBookOrderRepository>();

builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddSingleton<ICartService, CartService>();
builder.Services.AddSingleton<IClientService, ClientService>();
builder.Services.AddSingleton<IStockBookOrderService, StockBookOrderService>();
builder.Services.AddSingleton<IEventHandler<BookStockAmountUpdatedEvent>, BookStockAmountUpdatedEventHandler>();
builder.Services.AddSingleton<IEventDispatcher, EventDispatcher>();
builder.Services.AddSingleton<IStatisticsService, StatisticsService>();
builder.Services.AddSingleton<IAdvisorService, AdvisorService>();
builder.Services.AddSingleton<ILibraryService, LibraryService>();

#endregion

builder.Services.AddPagination(builder.Configuration);
builder.Services.AddRepositoryPatternWithResilience<ShopDbContext>(builder.Configuration);
builder.Services.AddDefaultResiliencePipeline(builder.Configuration, Configuration.DEFAULT_RESILIENCE_PIPELINE);
builder.Services.AddCustomHttpClientServiceWithResilience(builder.Configuration);
builder.Services.AddSharedFluentValidation(typeof(Program));
builder.Services.ConfigureCustomInvalidModelStateResponseControllers();

builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

#region Caching

builder.Services.AddOutputCache((options) =>
{
    options.AddPolicy("BasePolicy", new OutputCachePolicy());

    options.SetOutputCachePolicy("AdvisorPolicy", duration: TimeSpan.FromSeconds(10), type: typeof(AdvisorQueryRequest));

    options.SetOutputCachePolicy("CartPolicy", duration: TimeSpan.FromSeconds(3), useAuthId: true);

    options.SetOutputCachePolicy("ClientPolicy", duration: TimeSpan.FromSeconds(3), useAuthId: true);

    options.SetOutputCachePolicy("OrderPaginationPolicy", duration: TimeSpan.FromSeconds(3), useAuthId: true, type: typeof(GetOrdersFilter));

    options.SetOutputCachePolicy("StatisticsPolicy", duration: TimeSpan.FromSeconds(10), type: typeof(GetShopStatisticsRequest));

    options.SetOutputCachePolicy("StockBookOrderPaginationPolicy", duration: TimeSpan.FromSeconds(10), type: typeof(PaginationRequest));
});

#endregion

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwagger("Shop API");
}

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

if (app.Configuration[Configuration.EF_CREATE_DATABASE] == "true")
{
    await app.ConfigureDatabaseAsync<ShopDbContext>(CancellationToken.None);
}

app.UseSharedMiddleware();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
else
{
    app.UseSwagger("Shop API V1");
}

app.UseIdentity();

app.UseOutputCache();

app.MapControllers();

app.Run();

public partial class Program { }