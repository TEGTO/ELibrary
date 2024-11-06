using Authentication.Token;
using LibraryShopEntities.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Configurations;
using Testcontainers.PostgreSql;

namespace ShopApi.IntegrationTests.IntegrationTests
{
    public sealed class WebAppFactoryWrapper : IAsyncDisposable
    {
        public PostgreSqlContainer dbContainer;

        protected WebApplicationFactory<Program> WebApplicationFactory { get; private set; }

        public async Task<WebApplicationFactory<Program>> GetFactoryAsync()
        {
            if (WebApplicationFactory == null)
            {
                await InitializeContainersAsync();
                WebApplicationFactory = InitializeFactory();
            }
            return WebApplicationFactory;
        }
        public async ValueTask DisposeAsync()
        {
            if (WebApplicationFactory != null)
            {
                await dbContainer.StopAsync();

                await dbContainer.DisposeAsync();

                await WebApplicationFactory.DisposeAsync();
                WebApplicationFactory = null;
            }
        }

        private async Task InitializeContainersAsync()
        {
            dbContainer = new PostgreSqlBuilder()
                .WithImage("postgres:latest")
                .WithDatabase("elibrary-db")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();

            await dbContainer.StartAsync();

        }
        private WebApplicationFactory<Program> InitializeFactory()
        {
            return new WebApplicationFactory<Program>()
              .WithWebHostBuilder(builder =>
              {
                  builder.UseConfiguration(GetConfiguration());

                  builder.ConfigureTestServices(services =>
                  {
                      services.RemoveAll(typeof(IDbContextFactory<ShopDbContext>));

                      services.AddDbContextFactory<ShopDbContext>(options =>
                          options.UseNpgsql(dbContainer.GetConnectionString()));
                  });
              });
        }
        private IConfigurationRoot GetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ConnectionStrings:" + Configuration.SHOP_DATABASE_CONNECTION_STRING, dbContainer.GetConnectionString() },
                { Configuration.EF_CREATE_DATABASE, "true" },
                { Configuration.USE_CORS, "true" },
                { Configuration.SHOP_MAX_ORDER_AMOUNT, "99" },
                { JwtConfiguration.JWT_SETTINGS_KEY, "q57+LXDr4HtynNQaYVs7t50HwzvTNrWM2E/OepoI/D4=" },
                { JwtConfiguration.JWT_SETTINGS_ISSUER, "https://token.issuer.example.com" },
                { JwtConfiguration.JWT_SETTINGS_EXPIRY_IN_MINUTES, "30" },
                { JwtConfiguration.JWT_SETTINGS_AUDIENCE, "https://api.example.com" },
                { SharedConfiguration.MAX_PAGINATION_PAGE_SIZE, "99" },
            });
            return configurationBuilder.Build();
        }
    }
}
