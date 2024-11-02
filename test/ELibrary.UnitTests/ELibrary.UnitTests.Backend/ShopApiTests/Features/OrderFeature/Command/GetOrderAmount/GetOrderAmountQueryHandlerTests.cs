using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.ClientFeature.Services;
using ShopApi.Features.OrderFeature.Dtos;
using ShopApi.Features.OrderFeature.Services;

namespace ShopApi.Features.OrderFeature.Command.GetOrderAmount.Tests
{
    [TestFixture]
    internal class GetOrderAmountQueryHandlerTests
    {
        private Mock<IOrderService> orderServiceMock;
        private Mock<IClientService> clientServiceMock;
        private GetOrderAmountQueryHandler handler;

        [SetUp]
        public void SetUp()
        {
            orderServiceMock = new Mock<IOrderService>();
            clientServiceMock = new Mock<IClientService>();
            handler = new GetOrderAmountQueryHandler(orderServiceMock.Object, clientServiceMock.Object);
        }

        [Test]
        public void Handle_ClientNotFound_ThrowsInvalidDataException()
        {
            // Arrange
            clientServiceMock.Setup(x => x.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
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
            clientServiceMock.Setup(x => x.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            orderServiceMock.Setup(x => x.GetOrderAmountAsync(It.IsAny<GetOrdersFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(orderAmount);
            var command = new GetOrderAmountQuery("user-id", new GetOrdersFilter());
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(orderAmount));
            orderServiceMock.Verify(x => x.GetOrderAmountAsync(It.Is<GetOrdersFilter>(f => f.ClientId == client.Id), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task Handle_ClientFound_SetsClientIdInFilter()
        {
            // Arrange
            var client = new Client { Id = "client-id" };
            clientServiceMock.Setup(x => x.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            var command = new GetOrderAmountQuery("user-id", new GetOrdersFilter());
            // Act
            await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.That(command.Request.ClientId, Is.EqualTo(client.Id));
            orderServiceMock.Verify(x => x.GetOrderAmountAsync(It.Is<GetOrdersFilter>(f => f.ClientId == client.Id), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}