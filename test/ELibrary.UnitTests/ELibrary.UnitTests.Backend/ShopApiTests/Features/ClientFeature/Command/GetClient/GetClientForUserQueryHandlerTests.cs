using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.ClientFeature.Services;

namespace ShopApi.Features.ClientFeature.Command.GetClient.Tests
{
    [TestFixture]
    internal class GetClientForUserQueryHandlerTests
    {
        private Mock<IClientService> mockClientService;
        private Mock<IMapper> mockMapper;
        private GetClientForUserQueryHandler handler;

        [SetUp]
        public void SetUp()
        {
            mockClientService = new Mock<IClientService>();
            mockMapper = new Mock<IMapper>();
            handler = new GetClient.GetClientForUserQueryHandler(mockClientService.Object, mockMapper.Object);
        }

        [Test]
        public async Task Handle_ExistingUserId_ReturnsMappedClientResponse()
        {
            // Arrange
            var query = new GetClientForUserQuery("user123");
            var client = new Client { Id = "1", UserId = "user123", Name = "John Doe" };
            var expectedResponse = new ClientResponse { Id = "1", UserId = "user123", Name = "John Doe" };
            mockClientService.Setup(s => s.GetClientByUserIdAsync(query.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockMapper.Setup(m => m.Map<ClientResponse>(client)).Returns(expectedResponse);
            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Client, Is.EqualTo(expectedResponse));
            mockClientService.Verify(s => s.GetClientByUserIdAsync(query.UserId, It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(m => m.Map<ClientResponse>(client), Times.Once);
        }
        [Test]
        public async Task Handle_NonExistingUserId_ReturnsNullClient()
        {
            // Arrange
            var query = new GetClientForUserQuery("nonexistentUser");
            mockClientService.Setup(s => s.GetClientByUserIdAsync(query.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client)null);
            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Client);
            mockClientService.Verify(s => s.GetClientByUserIdAsync(query.UserId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}