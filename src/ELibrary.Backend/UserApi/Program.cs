using Authentication;
using Authentication.OAuth;
using Authentication.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Middlewares;
using Shared.Repositories;
using UserApi;
using UserApi.Services;
using UserApi.Services.Auth;
using UserApi.Services.OAuth;
using UserApi.Services.OAuth.Google;
using UserEntities.Data;
using UserEntities.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

#region Cors

bool.TryParse(builder.Configuration[Configuration.USE_CORS], out bool useCors);
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

if (useCors)
{
    builder.Services.AddApplicationCors(builder.Configuration, MyAllowSpecificOrigins, builder.Environment.IsDevelopment());
}

#endregion

builder.Services.AddDbContextFactory<UserIdentityDbContext>(builder.Configuration.GetConnectionString(Configuration.AUTH_DATABASE_CONNECTION_STRING)!, "UserApi");

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
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<GoogleOAuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IGoogleOAuthHttpClient, GoogleOAuthHttpClient>();
builder.Services.AddScoped<IUserOAuthCreationService, UserOAuthCreationService>();
builder.Services.AddScoped<IUserAuthenticationMethodService, UserAuthenticationMethodService>();
builder.Services.AddSingleton<IDatabaseRepository<UserIdentityDbContext>, DatabaseRepository<UserIdentityDbContext>>();

builder.Services.AddScoped(provider => new Dictionary<OAuthLoginProvider, IOAuthService>
    {
        { OAuthLoginProvider.Google, provider.GetService<GoogleOAuthService>()! },
    });

builder.Services.AddPaginationConfiguration(builder.Configuration);
builder.Services.AddRepositoryPatternWithResilience<UserIdentityDbContext>(builder.Configuration);

builder.Services.AddCustomHttpClientServiceWithResilience(builder.Configuration);

#endregion

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddMemoryCache();

builder.Services.AddSharedFluentValidation(typeof(Program));

builder.Services.ConfigureCustomInvalidModelStateResponseControllers();

var app = builder.Build();

if (app.Configuration[Configuration.EF_CREATE_DATABASE] == "true")
{
    await app.ConfigureDatabaseAsync<UserIdentityDbContext>(CancellationToken.None);
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