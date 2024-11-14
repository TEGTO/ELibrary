using Authentication.Token;
using AutoMapper;
using Caching.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
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

            factory = await wrapper.GetFactoryAsync();

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