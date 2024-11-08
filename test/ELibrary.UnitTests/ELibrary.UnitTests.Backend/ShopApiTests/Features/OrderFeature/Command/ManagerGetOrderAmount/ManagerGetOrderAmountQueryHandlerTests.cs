using LibraryShopEntities.Filters;
using Moq;
using ShopApi.Features.OrderFeature.Services;

namespace ShopApi.Features.OrderFeature.Command.ManagerGetOrderAmount.Tests
{
    [TestFixture]
    internal class ManagerGetOrderAmountQueryHandlerTests
    {
        private Mock<IOrderService> orderServiceMock;
        private ManagerGetOrderAmountQueryHandler handler;

        [SetUp]
        public void SetUp()
        {
            orderServiceMock = new Mock<IOrderService>();
            handler = new ManagerGetOrderAmountQueryHandler(orderServiceMock.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsOrderAmount()
        {
            // Arrange
            var filter = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            var request = new ManagerGetOrderAmountQuery(filter);
            var expectedOrderAmount = 5;
            orderServiceMock
                .Setup(x => x.GetOrderAmountAsync(filter, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedOrderAmount);
            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(expectedOrderAmount));
            orderServiceMock.Verify(x => x.GetOrderAmountAsync(filter, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}