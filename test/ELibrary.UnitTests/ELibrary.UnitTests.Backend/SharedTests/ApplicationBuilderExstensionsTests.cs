using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Serilog;

namespace Shared.Tests
{
    [TestFixture]
    internal class ApplicationBuilderExtensionsTests
    {
        private Mock<IApplicationBuilder> appBuilderMock;
        private Mock<IServiceProvider> serviceProviderMock;
        private Mock<IServiceScope> serviceScopeMock;
        private Mock<IServiceScopeFactory> serviceScopeFactoryMock;
        private Mock<IConfiguration> configurationMock;
        private Mock<ILogger> loggerMock;
        private CancellationToken cancellationToken;

        [SetUp]
        public void SetUp()
        {
            appBuilderMock = new Mock<IApplicationBuilder>();
            serviceProviderMock = new Mock<IServiceProvider>();
            serviceScopeMock = new Mock<IServiceScope>();
            serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            configurationMock = new Mock<IConfiguration>();
            loggerMock = new Mock<ILogger>();

            serviceScopeFactoryMock.Setup(factory => factory.CreateScope()).Returns(serviceScopeMock.Object);
            serviceScopeMock.Setup(scope => scope.ServiceProvider).Returns(serviceProviderMock.Object);

            serviceProviderMock.Setup(provider => provider.GetService(typeof(IServiceScopeFactory))).Returns(serviceScopeFactoryMock.Object);
            serviceProviderMock.Setup(provider => provider.GetService(typeof(IConfiguration))).Returns(configurationMock.Object);
            serviceProviderMock.Setup(provider => provider.GetService(typeof(ILogger))).Returns(loggerMock.Object);

            appBuilderMock.Setup(app => app.ApplicationServices).Returns(serviceProviderMock.Object);

            cancellationToken = new CancellationToken();
        }
    }
}