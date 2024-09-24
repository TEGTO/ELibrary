using Authentication;
using LibraryShopEntities.Data;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Middlewares;
using Shared.Repositories;
using ShopApi;
using ShopApi.Services;
using ShopApi.Services.Facades;

var builder = WebApplication.CreateBuilder(args);

#region Cors

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddApplicationCors(builder.Configuration, MyAllowSpecificOrigins, builder.Environment.IsDevelopment());

#endregion

builder.Services.AddDbContextFactory<LibraryShopDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(Configuration.SHOP_DATABASE_CONNECTION_STRING)));

#region Identity & Authentication

builder.Services.ConfigureIdentityServices(builder.Configuration);

#endregion

#region Project Services 

builder.Services.AddSingleton<IClientService, ClientService>();
builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddSingleton<ICartService, CartService>();
builder.Services.AddSingleton<IOrderManager, OrderManager>();
builder.Services.AddSingleton<IClientManager, ClientManager>();

builder.Services.AddSingleton<IDatabaseRepository<LibraryShopDbContext>, DatabaseRepository<LibraryShopDbContext>>();
#endregion

builder.Services.AddMemoryCache();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddSharedFluentValidation(typeof(Program));

builder.Services.ConfigureCustomInvalidModelStateResponseControllers();

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);
app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
