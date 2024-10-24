using Authentication.Identity;
using Authentication.OAuth;
using Authentication.OAuth.Google;
using Authentication.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authentication
{
    public static class CustomAuthExtension
    {
        public static void ConfigureIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings()
            {
                Key = configuration[JwtConfiguration.JWT_SETTINGS_KEY]!,
                Audience = configuration[JwtConfiguration.JWT_SETTINGS_AUDIENCE]!,
                Issuer = configuration[JwtConfiguration.JWT_SETTINGS_ISSUER]!,
                ExpiryInMinutes = Convert.ToDouble(configuration[JwtConfiguration.JWT_SETTINGS_EXPIRY_IN_MINUTES]!),
            };
            var googleOAuthSettings = new GoogleOAuthSettings()
            {
                ClientId = configuration[OAuthConfiguration.GOOGLE_OAUTH_CLIENT_ID]!,
                ClientSecret = configuration[OAuthConfiguration.GOOGLE_OAUTH_CLIENT_SECRET]!,
                Scope = configuration[OAuthConfiguration.GOOGLE_OAUTH_SCOPE]!,
            };

            services.AddSingleton(jwtSettings);
            services.AddSingleton(googleOAuthSettings);

            services.AddScoped<IGoogleTokenValidator, GoogleTokenValidator>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy.REQUIRE_CLIENT_ROLE,
                    policy => policy.RequireRole(Roles.CLIENT, Roles.MANAGER, Roles.ADMINISTRATOR));
                options.AddPolicy(Policy.REQUIRE_MANAGER_ROLE,
                    policy => policy.RequireRole(Roles.MANAGER, Roles.ADMINISTRATOR));
                options.AddPolicy(Policy.REQUIRE_ADMIN_ROLE,
                    policy => policy.RequireRole(Roles.ADMINISTRATOR));
            });
            services.AddCustomAuthentication(jwtSettings);
        }
        public static void AddCustomAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
        }
    }
}