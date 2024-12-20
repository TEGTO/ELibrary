﻿using Authentication.Token;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pagination;
using Testcontainers.PostgreSql;
using UserEntities.Data;

namespace UserApi.IntegrationTests
{
    public sealed class WebAppFactoryWrapper : IAsyncDisposable
    {
        private PostgreSqlContainer? DbContainer { get; set; }
        private WebApplicationFactory<Program>? WebApplicationFactory { get; set; }

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
            if (DbContainer != null)
            {
                await DbContainer.StopAsync();
                await DbContainer.DisposeAsync();
            }

            if (WebApplicationFactory != null)
            {
                await WebApplicationFactory.DisposeAsync();
                WebApplicationFactory = null;
            }
        }

        private async Task InitializeContainersAsync()
        {
            DbContainer = new PostgreSqlBuilder()
                .WithImage("postgres:latest")
                .WithDatabase("elibrary-db")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();

            await DbContainer.StartAsync();
        }
        private WebApplicationFactory<Program> InitializeFactory()
        {
            return new WebApplicationFactory<Program>()
              .WithWebHostBuilder(builder =>
              {
                  builder.UseConfiguration(GetConfiguration());

                  builder.ConfigureTestServices(services =>
                  {
                      services.RemoveAll(typeof(IDbContextFactory<UserIdentityDbContext>));

                      services.AddDbContextFactory<UserIdentityDbContext>(options =>
                          options.UseNpgsql(DbContainer?.GetConnectionString()));
                  });
              });
        }
        private IConfigurationRoot GetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ConnectionStrings:" + Configuration.AUTH_DATABASE_CONNECTION_STRING, DbContainer?.GetConnectionString() },
                { Configuration.AUTH_REFRESH_TOKEN_EXPIRY_IN_DAYS, "7" },
                { Configuration.EF_CREATE_DATABASE, "true" },
                { JwtConfiguration.JWT_SETTINGS_KEY, "q57+LXDr4HtynNQaYVs7t50HwzvTNrWM2E/OepoI/D4=" },
                { JwtConfiguration.JWT_SETTINGS_ISSUER, "https://token.issuer.example.com" },
                { JwtConfiguration.JWT_SETTINGS_EXPIRY_IN_MINUTES, "30" },
                { JwtConfiguration.JWT_SETTINGS_AUDIENCE, "https://api.example.com" },
                { PaginationConfiguration.MAX_PAGINATION_PAGE_SIZE, "99" },
            });

            return configurationBuilder.Build();
        }
    }
}
