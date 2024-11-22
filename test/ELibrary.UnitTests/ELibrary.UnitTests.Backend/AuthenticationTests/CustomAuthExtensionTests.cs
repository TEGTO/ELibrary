using Authentication.Identity;
using Authentication.OAuth;
using Authentication.OAuth.Google;
using Authentication.Token;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Tests
{
    [TestFixture]
    internal class CustomAuthExtensionTests
    {
        private IServiceCollection services;
        private IConfiguration configuration;
        private JwtSettings expectedJwtSettings;
        private GoogleOAuthSettings expectedGoogleOAuthSettings;

        [SetUp]
        public void SetUp()
        {
            services = new ServiceCollection();

            expectedJwtSettings = new JwtSettings
            {
                Key = "A very secret key",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExpiryInMinutes = 60
            };

            expectedGoogleOAuthSettings = new GoogleOAuthSettings()
            {
                ClientId = "ClientId",
                ClientSecret = "Some google secret",
                Scope = "profile.com"
            };

            var inMemorySettings = new Dictionary<string, string>
            {
                { JwtConfiguration.JWT_SETTINGS_KEY, expectedJwtSettings.Key },
                { JwtConfiguration.JWT_SETTINGS_AUDIENCE, expectedJwtSettings.Audience },
                { JwtConfiguration.JWT_SETTINGS_ISSUER, expectedJwtSettings.Issuer },
                { JwtConfiguration.JWT_SETTINGS_EXPIRY_IN_MINUTES, expectedJwtSettings.ExpiryInMinutes.ToString() },

                { OAuthConfiguration.GOOGLE_OAUTH_CLIENT_ID, expectedGoogleOAuthSettings.ClientId },
                { OAuthConfiguration.GOOGLE_OAUTH_CLIENT_SECRET, expectedGoogleOAuthSettings.ClientSecret },
                { OAuthConfiguration.GOOGLE_OAUTH_SCOPE, expectedGoogleOAuthSettings.Scope },
            };

            configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings!).Build();

            services.AddSingleton(configuration);
            services.AddAuthentication();
            services.AddAuthorization();
        }
        [Test]
        public void ConfigureIdentityServices_ShouldAuthSettingsAsSingletons()
        {
            // Act
            services.ConfigureIdentityServices(configuration);

            //Act
            var serviceProvider = services.BuildServiceProvider();
            var jwtSettings = serviceProvider.GetRequiredService<JwtSettings>();
            var googleOAuthSettings = serviceProvider.GetRequiredService<GoogleOAuthSettings>();

            // Assert
            Assert.That(jwtSettings.Key, Is.EqualTo(expectedJwtSettings.Key));
            Assert.That(jwtSettings.Issuer, Is.EqualTo(expectedJwtSettings.Issuer));
            Assert.That(jwtSettings.Audience, Is.EqualTo(expectedJwtSettings.Audience));
            Assert.That(jwtSettings.ExpiryInMinutes, Is.EqualTo(expectedJwtSettings.ExpiryInMinutes));

            Assert.That(googleOAuthSettings.ClientId, Is.EqualTo(expectedGoogleOAuthSettings.ClientId));
            Assert.That(googleOAuthSettings.ClientSecret, Is.EqualTo(expectedGoogleOAuthSettings.ClientSecret));
            Assert.That(googleOAuthSettings.Scope, Is.EqualTo(expectedGoogleOAuthSettings.Scope));
        }
        [Test]
        public void ConfigureIdentityServices_ShouldConfigureAuthorization()
        {
            // Act
            services.ConfigureIdentityServices(configuration);

            //Act
            var serviceProvider = services.BuildServiceProvider();
            var authorizationOptions = serviceProvider.GetRequiredService<IOptions<AuthorizationOptions>>().Value;

            // Assert
            Assert.That(authorizationOptions, Is.Not.Null);
            Assert.That(authorizationOptions.GetPolicy(Policy.REQUIRE_CLIENT_ROLE), Is.Not.Null);
            Assert.That(authorizationOptions.GetPolicy(Policy.REQUIRE_MANAGER_ROLE), Is.Not.Null);
            Assert.That(authorizationOptions.GetPolicy(Policy.REQUIRE_ADMIN_ROLE), Is.Not.Null);
        }
        [Test]
        public void ConfigureIdentityServices_ShouldConfigureOAuthTokenValidators()
        {
            // Act
            services.ConfigureIdentityServices(configuration);

            //Act
            var serviceProvider = services.BuildServiceProvider();
            var googleTokenValidator = serviceProvider.GetRequiredService<IGoogleTokenValidator>();

            // Assert
            Assert.That(googleTokenValidator, Is.Not.Null);
        }
        [Test]
        public void ConfigureIdentityServices_ShouldConfigureCustomJwtAuthentication()
        {
            // Act
            services.ConfigureIdentityServices(configuration);

            //Act
            var serviceProvider = services.BuildServiceProvider();
            var authenticationOptions = serviceProvider.GetRequiredService<IOptions<AuthenticationOptions>>().Value;
            var jwtBearerOptions = serviceProvider.GetRequiredService<IOptionsSnapshot<JwtBearerOptions>>()
                .Get(JwtBearerDefaults.AuthenticationScheme);
            var tokenValidationParameters = jwtBearerOptions.TokenValidationParameters;

            // Assert
            Assert.That(authenticationOptions.DefaultAuthenticateScheme, Is.EqualTo(JwtBearerDefaults.AuthenticationScheme));
            Assert.That(authenticationOptions.DefaultChallengeScheme, Is.EqualTo(JwtBearerDefaults.AuthenticationScheme));
            Assert.That(authenticationOptions.DefaultScheme, Is.EqualTo(JwtBearerDefaults.AuthenticationScheme));

            Assert.That(tokenValidationParameters.ValidIssuer, Is.EqualTo(expectedJwtSettings.Issuer));
            Assert.That(tokenValidationParameters.ValidAudience, Is.EqualTo(expectedJwtSettings.Audience));
            Assert.That(tokenValidationParameters.IssuerSigningKey, Is.TypeOf<SymmetricSecurityKey>());

            Assert.IsTrue(tokenValidationParameters.ValidateIssuer);
            Assert.IsTrue(tokenValidationParameters.ValidateAudience);
            Assert.IsTrue(tokenValidationParameters.ValidateLifetime);
            Assert.IsTrue(tokenValidationParameters.ValidateIssuerSigningKey);
        }
    }
}