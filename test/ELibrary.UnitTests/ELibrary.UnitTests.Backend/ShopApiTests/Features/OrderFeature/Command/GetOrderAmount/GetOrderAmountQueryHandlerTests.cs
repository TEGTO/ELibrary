using LibraryShopEntities.Domain.Entities.Shop;
using LibraryShopEntities.Filters;
using Moq;
using ShopApi.Features.ClientFeature.Services;
using ShopApi.Features.OrderFeature.Services;

namespace ShopApi.Features.OrderFeature.Command.GetOrderAmount.Tests
{
    [TestFixture]
    internal class GetOrderAmountQueryHandlerTests
    {
        private Mock<IOrderService> mockOrderService;
        private Mock<IClientService> mockClientService;
        private GetOrderAmountQueryHandler handler;

        [SetUp]
        public void SetUp()
        {
            mockOrderService = new Mock<IOrderService>();
            mockClientService = new Mock<IClientService>();
            handler = new GetOrderAmountQueryHandler(mockOrderService.Object, mockClientService.Object);
        }

        [Test]
        public void Handle_ClientNotFound_ThrowsInvalidDataException()
        {
            // Arrange
            mockClientService.Setup(x => x.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client)null);
            var command = new GetOrderAmountQuery("user-id", new GetOrdersFilter());
            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidDataException>(() => handler.Handle(command, CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("Client is not found!"));
        }
        [Test]
        public async Task Handle_ValidClient_ReturnsOrderAmount()
        {
            // Arrange
            var client = new Client { Id = "client-id" };
            var orderAmount = 5;
            mockClientService.Setup(x => x.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockOrderService.Setup(x => x.GetOrderAmountAsync(It.IsAny<GetOrdersFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(orderAmount);
            var command = new GetOrderAmountQuery("user-id", new GetOrdersFilter());
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(orderAmount));
            mockOrderService.Verify(x => x.GetOrderAmountAsync(It.Is<GetOrdersFilter>(f => f.ClientId == client.Id), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task Handle_ClientFound_SetsClientIdInFilter()
        {
            // Arrange
            var client = new Client { Id = "client-id" };
            mockClientService.Setup(x => x.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            var command = new GetOrderAmountQuery("user-id", new GetOrdersFilter());
            // Act
            await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.That(command.Request.ClientId, Is.EqualTo(client.Id));
            mockOrderService.Verify(x => x.GetOrderAmountAsync(It.Is<GetOrdersFilter>(f => f.ClientId == client.Id), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}