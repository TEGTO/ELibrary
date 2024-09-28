using Authentication;
using Authentication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Middlewares;
using Shared.Repositories;
using UserApi;
using UserApi.Services;
using UserEntities.Data;
using UserEntities.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

#region Cors

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddApplicationCors(builder.Configuration, MyAllowSpecificOrigins, builder.Environment.IsDevelopment());

#endregion

builder.Services.AddDbContextFactory<UserIdentityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(Configuration.AUTH_DATABASE_CONNECTION_STRING), b =>
        b.MigrationsAssembly("UserApi")));

#region Identity 

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<UserIdentityDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureIdentityServices(builder.Configuration);
builder.Services.AddScoped<ITokenHandler, JwtHandler>();
#endregion

#region Project Services 

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddSingleton<IDatabaseRepository<UserIdentityDbContext>, DatabaseRepository<UserIdentityDbContext>>();

#endregion

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddMemoryCache();

builder.Services.AddSharedFluentValidation(typeof(Program));

builder.Services.ConfigureCustomInvalidModelStateResponseControllers();

var app = builder.Build();

if (app.Configuration[Configuration.EF_CREATE_DATABASE] == "true")
{
    await app.ConfigureDatabaseAsync<UserIdentityDbContext>(CancellationToken.None);
}

app.UseCors(MyAllowSpecificOrigins);
app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();