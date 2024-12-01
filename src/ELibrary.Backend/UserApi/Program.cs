using Authentication;
using Authentication.OAuth;
using Authentication.Token;
using DatabaseControl;
using ExceptionHandling;
using Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pagination;
using Shared;
using UserApi;
using UserApi.Services;
using UserApi.Services.Auth;
using UserApi.Services.OAuth;
using UserApi.Services.OAuth.Google;
using UserEntities.Data;
using UserEntities.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<UserIdentityDbContext>(builder.Configuration.GetConnectionString(Configuration.AUTH_DATABASE_CONNECTION_STRING)!, "UserApi");
builder.Host.AddLogging();

#region Identity & Authentication

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
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<GoogleOAuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IGoogleOAuthHttpClient, GoogleOAuthHttpClient>();
builder.Services.AddScoped<IUserOAuthCreationService, UserOAuthCreationService>();
builder.Services.AddScoped<IUserAuthenticationMethodService, UserAuthenticationMethodService>();
builder.Services.AddScoped(provider => new Dictionary<OAuthLoginProvider, IOAuthService>
    {
        { OAuthLoginProvider.Google, provider.GetService<GoogleOAuthService>()! },
    });

#endregion

builder.Services.AddPagination(builder.Configuration);

builder.Services.AddRepositoryWithResilience<UserIdentityDbContext>(builder.Configuration);

builder.Services.AddCustomHttpClientServiceWithResilience(builder.Configuration);
builder.Services.AddSharedFluentValidation(typeof(Program));
builder.Services.ConfigureCustomInvalidModelStateResponseControllers();

builder.Services.AddPagination(builder.Configuration);
builder.Services.AddRepositoryWithResilience<UserIdentityDbContext>(builder.Configuration);
builder.Services.AddCustomHttpClientServiceWithResilience(builder.Configuration);
builder.Services.AddSharedFluentValidation(typeof(Program));
builder.Services.ConfigureCustomInvalidModelStateResponseControllers();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwagger("User API");
}

var app = builder.Build();

if (app.Configuration[Configuration.EF_CREATE_DATABASE] == "true")
{
    await app.ConfigureDatabaseAsync<UserIdentityDbContext>(CancellationToken.None);
}

app.UseSharedMiddleware();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
else
{
    app.UseSwagger("User API V1");
}

app.UseIdentity();

app.MapControllers();

app.Run();

public partial class Program { }
