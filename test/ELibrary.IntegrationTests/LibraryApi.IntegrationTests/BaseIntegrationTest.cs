using Authentication.Token;
using AutoMapper;
using Caching.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace LibraryApi.IntegrationTests
{
    [TestFixture]
    public abstract class BaseIntegrationTest
    {
        protected HttpClient client;
        protected JwtSettings settings;
        protected IMapper mapper;
        protected Mock<ICachingHelper> mockCachingHelper;
        private WebAppFactoryWrapper wrapper;
        private WebApplicationFactory<Program> factory;
        private IServiceScope scope;

        [OneTimeSetUp]
        public async Task GlobalSetup()
        {
            wrapper = new WebAppFactoryWrapper();

            factory = (await wrapper.GetFactoryAsync()).WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll(typeof(ICachingHelper));

                    mockCachingHelper = new Mock<ICachingHelper>();

                    services.AddSingleton(mockCachingHelper.Object);

                    mockCachingHelper.Setup(x => x.GetCacheKey(It.IsAny<string>(), It.IsAny<HttpContext>()))
                    .Returns(() => Guid.NewGuid().ToString());
                });
            });

            InitializeServices();
        }
        [OneTimeTearDown]
        public async Task GlobalTearDown()
        {
            scope.Dispose();
            client.Dispose();
            await factory.DisposeAsync();
            await wrapper.DisposeAsync();
        }

        private void InitializeServices()
        {
            scope = factory.Services.CreateScope();
            client = factory.CreateClient();
            settings = factory.Services.GetRequiredService<JwtSettings>();
            mapper = factory.Services.GetRequiredService<IMapper>();
        }
    }
}