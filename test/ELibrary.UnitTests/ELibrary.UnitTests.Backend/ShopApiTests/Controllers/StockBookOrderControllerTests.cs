using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.Dtos;
using ShopApi.Features.StockBookOrderFeature.Dtos;
using ShopApi.Features.StockBookOrderFeature.Services;

namespace ShopApi.Controllers.Tests
{
    [TestFixture]
    public class StockBookOrderControllerTests
    {
        private Mock<IMapper> mapperMock;
        private Mock<IStockBookOrderService> stockBookOrderServiceMock;
        private StockBookOrderController stockBookOrderController;
        private CancellationToken cancellationToken;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            stockBookOrderServiceMock = new Mock<IStockBookOrderService>();
            stockBookOrderController = new StockBookOrderController(mapperMock.Object, stockBookOrderServiceMock.Object);
            cancellationToken = CancellationToken.None;
        }

        [Test]
        public async Task GetStockOrderById_WhenOrderExists_ReturnsMappedOrder()
        {
            // Arrange
            var stockBookOrder = new StockBookOrder { Id = 1, ClientId = "client-1" };
            var stockBookOrderResponse = new StockBookOrderResponse { Id = 1 };
            stockBookOrderServiceMock
                .Setup(s => s.GetStockBookOrderByIdAsync(1, cancellationToken))
                .ReturnsAsync(stockBookOrder);
            mapperMock
                .Setup(m => m.Map<StockBookOrderResponse>(stockBookOrder))
                .Returns(stockBookOrderResponse);
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
            stockBookOrderServiceMock
                .Setup(s => s.GetStockBookOrderByIdAsync(1, cancellationToken))
                .ReturnsAsync((StockBookOrder)null);
            var result = await stockBookOrderController.GetStockOrderById(1, cancellationToken);
            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }
        [Test]
        public async Task GetStockOrderPaginated_WithPagination_ReturnsMappedOrders()
        {
            // Arrange
            var paginationRequest = new PaginationRequest { PageNumber = 1, PageSize = 2 };
            var stockBookOrders = new List<StockBookOrder>
            {
                new StockBookOrder { Id = 1 },
                new StockBookOrder { Id = 2 }
            };
            var stockBookOrderResponses = new List<StockBookOrderResponse>
            {
                new StockBookOrderResponse { Id = 1 },
                new StockBookOrderResponse { Id = 2 }
            };

            stockBookOrderServiceMock
                .Setup(s => s.GetPaginatedStockBookOrdersAsync(paginationRequest, cancellationToken))
                .ReturnsAsync(stockBookOrders);
            mapperMock
                .Setup(m => m.Map<StockBookOrderResponse>(stockBookOrders[0])).Returns(stockBookOrderResponses[0]);
            mapperMock
                .Setup(m => m.Map<StockBookOrderResponse>(stockBookOrders[1])).Returns(stockBookOrderResponses[1]);
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
        public async Task GetStockOrderAmount_WhenCalled_ReturnsOkWithAmount()
        {
            // Arrange
            var expectedAmount = 5;
            stockBookOrderServiceMock
                .Setup(s => s.GetStockBookAmountAsync(cancellationToken))
                .ReturnsAsync(expectedAmount);
            // Act
            var result = await stockBookOrderController.GetStockOrderAmount(cancellationToken);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedAmount));
            stockBookOrderServiceMock.Verify(s => s.GetStockBookAmountAsync(cancellationToken), Times.Once);
        }
        [Test]
        public async Task CreateStockBookOrder_ValidRequest_ReturnsCreatedOrder()
        {
            // Arrange
            var createRequest = new CreateStockBookOrderRequest();
            var stockBookOrder = new StockBookOrder { Id = 1 };
            var stockBookOrderResponse = new StockBookOrderResponse { Id = 1 };
            mapperMock
                .Setup(m => m.Map<StockBookOrder>(createRequest))
                .Returns(stockBookOrder);
            stockBookOrderServiceMock
                .Setup(s => s.AddStockBookOrderAsync(stockBookOrder, cancellationToken))
                .ReturnsAsync(stockBookOrder);
            mapperMock
                .Setup(m => m.Map<StockBookOrderResponse>(stockBookOrder))
                .Returns(stockBookOrderResponse);
            // Act
            var result = await stockBookOrderController.CreateStockBookOrder(createRequest, cancellationToken);
            // Assert
            Assert.IsInstanceOf<CreatedResult>(result.Result);
            var okResult = result.Result as CreatedResult;
            Assert.That(okResult?.Value, Is.EqualTo(stockBookOrderResponse));
        }
    }
}