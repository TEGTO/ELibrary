using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.Domain.Dtos;
using ShopApi.Features.StockBookOrderFeature.Command.CreateStockBookOrder;
using ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderAmount;
using ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderById;
using ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderPaginated;
using ShopApi.Features.StockBookOrderFeature.Dtos;

namespace ShopApi.Controllers.Tests
{
    [TestFixture]
    internal class StockBookOrderControllerTests
    {
        private Mock<IMediator> mediatorMock;
        private StockBookOrderController stockBookOrderController;
        private CancellationToken cancellationToken;

        [SetUp]
        public void SetUp()
        {
            mediatorMock = new Mock<IMediator>();
            stockBookOrderController = new StockBookOrderController(mediatorMock.Object);
            cancellationToken = CancellationToken.None;
        }

        [Test]
        public async Task GetStockOrderById_WhenOrderExists_ReturnsOrder()
        {
            // Arrange
            var stockBookOrderResponse = new StockBookOrderResponse { Id = 1 };
            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetStockOrderByIdQuery>(), cancellationToken))
                .ReturnsAsync(stockBookOrderResponse);
            // Act
            var result = await stockBookOrderController.GetStockOrderById(1, cancellationToken);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(stockBookOrderResponse));
        }
        [Test]
        public async Task GetStockOrderById_WhenOrderDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetStockOrderByIdQuery>(), cancellationToken))
                .ReturnsAsync((StockBookOrderResponse)null);
            // Act
            var result = await stockBookOrderController.GetStockOrderById(1, cancellationToken);
            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
        [Test]
        public async Task GetStockOrderAmount_WhenCalled_ReturnsOkWithAmount()
        {
            // Arrange
            var expectedAmount = 5;
            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetStockOrderAmountQuery>(), cancellationToken))
                .ReturnsAsync(expectedAmount);
            // Act
            var result = await stockBookOrderController.GetStockOrderAmount(cancellationToken);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedAmount));
            mediatorMock.Verify(m => m.Send(It.IsAny<GetStockOrderAmountQuery>(), cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetStockOrderPaginated_WithPagination_ReturnsPaginatedOrders()
        {
            // Arrange
            var paginationRequest = new PaginationRequest { PageNumber = 1, PageSize = 2 };
            var stockBookOrderResponses = new List<StockBookOrderResponse>
            {
                new StockBookOrderResponse { Id = 1 },
                new StockBookOrderResponse { Id = 2 }
            };
            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetStockOrderPaginatedQuery>(), cancellationToken))
                .ReturnsAsync(stockBookOrderResponses);
            // Act
            var result = await stockBookOrderController.GetStockOrderPaginated(paginationRequest, cancellationToken);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var returnedOrders = okResult?.Value as IEnumerable<StockBookOrderResponse>;
            Assert.That(returnedOrders.Count(), Is.EqualTo(2));
            Assert.That(returnedOrders.ElementAt(0).Id, Is.EqualTo(1));
            Assert.That(returnedOrders.ElementAt(1).Id, Is.EqualTo(2));
        }
        [Test]
        public async Task CreateStockBookOrder_ValidRequest_ReturnsCreatedOrder()
        {
            // Arrange
            var createRequest = new CreateStockBookOrderRequest();
            var stockBookOrderResponse = new StockBookOrderResponse { Id = 1 };
            mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateStockBookOrderCommand>(), cancellationToken))
                .ReturnsAsync(stockBookOrderResponse);
            // Act
            var result = await stockBookOrderController.CreateStockBookOrder(createRequest, cancellationToken);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(stockBookOrderResponse));
        }
    }
}