using Moq;
using ShopApi.Features.StockBookOrderFeature.Services;

namespace ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderAmount.Tests
{
    [TestFixture]
    internal class GetStockOrderAmountQueryHandlerTests
    {
        private Mock<IStockBookOrderService> stockBookOrderServiceMock;
        private GetStockOrderAmountQueryHandler handler;
        private CancellationToken cancellationToken;

        [SetUp]
        public void SetUp()
        {
            stockBookOrderServiceMock = new Mock<IStockBookOrderService>();
            handler = new GetStockOrderAmountQueryHandler(stockBookOrderServiceMock.Object);
            cancellationToken = new CancellationToken();
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsStockOrderAmount()
        {
            // Arrange
            int expectedAmount = 100;
            stockBookOrderServiceMock.Setup(s => s.GetStockBookAmountAsync(cancellationToken))
                .ReturnsAsync(expectedAmount);
            var query = new GetStockOrderAmountQuery();
            // Act
            var result = await handler.Handle(query, cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(expectedAmount));
            stockBookOrderServiceMock.Verify(s => s.GetStockBookAmountAsync(cancellationToken), Times.Once);
        }
        [Test]
        public async Task Handle_ServiceReturnsZero_ReturnsZero()
        {
            // Arrange
            stockBookOrderServiceMock.Setup(s => s.GetStockBookAmountAsync(cancellationToken))
                .ReturnsAsync(0);
            var query = new GetStockOrderAmountQuery();
            // Act
            var result = await handler.Handle(query, cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(0));
            stockBookOrderServiceMock.Verify(s => s.GetStockBookAmountAsync(cancellationToken), Times.Once);
        }
    }
}