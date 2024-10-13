using EventSourcing;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using MockQueryable.Moq;
using Moq;
using Shared.Domain.Dtos;
using Shared.Repositories;
using ShopApi.Features.StockBookOrderFeature.Models;

namespace ShopApi.Features.StockBookOrderFeature.Services.Tests
{
    [TestFixture]
    public class StockBookOrderServiceTests
    {
        private Mock<IDatabaseRepository<LibraryShopDbContext>> repositoryMock;
        private Mock<IEventDispatcher> eventDispatcherMock;
        private StockBookOrderService stockBookOrderService;
        private CancellationToken cancellationToken;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IDatabaseRepository<LibraryShopDbContext>>();
            eventDispatcherMock = new Mock<IEventDispatcher>();
            stockBookOrderService = new StockBookOrderService(repositoryMock.Object, eventDispatcherMock.Object);
            cancellationToken = CancellationToken.None;
        }
        private IQueryable<T> GetDbSetMock<T>(List<T> data) where T : class
        {
            return data.AsQueryable().BuildMock();
        }

        [Test]
        public async Task AddStockBookOrderAsync_AddsStockBookOrderAndDispatchesEvent()
        {
            // Arrange
            var stockBookOrder = new StockBookOrder();
            var newStockBookOrder = new StockBookOrder();
            var queryable = new List<StockBookOrder>() { stockBookOrder }.AsQueryable().BuildMock();
            repositoryMock.Setup(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken)).ReturnsAsync(queryable);
            repositoryMock.Setup(r => r.AddAsync(stockBookOrder, cancellationToken)).ReturnsAsync(newStockBookOrder);
            eventDispatcherMock.Setup(e => e.DispatchAsync(It.IsAny<BookStockAmountUpdatedEvent>(), cancellationToken)).Returns(Task.CompletedTask);
            // Act
            var result = await stockBookOrderService.AddStockBookOrderAsync(stockBookOrder, cancellationToken);
            // Assert
            Assert.That(result.Id, Is.EqualTo(newStockBookOrder.Id));
            repositoryMock.Verify(r => r.AddAsync(stockBookOrder, cancellationToken), Times.Once);
            eventDispatcherMock.Verify(e => e.DispatchAsync(It.IsAny<BookStockAmountUpdatedEvent>(), cancellationToken), Times.Once);
        }
        [Test]
        public async Task AddStockBookOrderAsyncFromOrderAsync_CreatesStockBookOrderWithNegativeChangeAmount()
        {
            // Arrange
            var order = CreateOrder();
            var stockBookOrder = new StockBookOrder();
            repositoryMock.Setup(r => r.AddAsync(It.IsAny<StockBookOrder>(), cancellationToken)).ReturnsAsync(stockBookOrder);
            var queryable = new List<StockBookOrder>() { stockBookOrder }.AsQueryable().BuildMock();
            repositoryMock.Setup(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken)).ReturnsAsync(queryable);
            eventDispatcherMock.Setup(e => e.DispatchAsync(It.IsAny<BookStockAmountUpdatedEvent>(), cancellationToken)).Returns(Task.CompletedTask);
            // Act
            await stockBookOrderService.AddStockBookOrderAsyncFromOrderAsync(order, StockBookOrderType.ClientOrder, cancellationToken);
            // Assert
            repositoryMock.Verify(r => r.AddAsync(It.Is<StockBookOrder>(s => s.StockBookChanges[0].ChangeAmount == -order.OrderAmount), cancellationToken), Times.Once);
            repositoryMock.Verify(r => r.AddAsync(It.Is<StockBookOrder>(s => s.Type == StockBookOrderType.ClientOrder), cancellationToken), Times.Once);
        }
        [Test]
        public async Task AddStockBookOrderAsyncFromCanceledOrderAsync_CreatesStockBookOrderWithPositiveChangeAmount()
        {
            // Arrange
            var order = CreateOrder();
            var stockBookOrder = new StockBookOrder();
            repositoryMock.Setup(r => r.AddAsync(It.IsAny<StockBookOrder>(), cancellationToken)).ReturnsAsync(stockBookOrder);
            var queryable = new List<StockBookOrder>() { stockBookOrder }.AsQueryable().BuildMock();
            repositoryMock.Setup(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken)).ReturnsAsync(queryable);
            eventDispatcherMock.Setup(e => e.DispatchAsync(It.IsAny<BookStockAmountUpdatedEvent>(), cancellationToken)).Returns(Task.CompletedTask);
            // Act
            await stockBookOrderService.AddStockBookOrderAsyncFromCanceledOrderAsync(order, StockBookOrderType.ClientOrderCancel, cancellationToken);
            // Assert
            repositoryMock.Verify(r => r.AddAsync(It.Is<StockBookOrder>(s => s.StockBookChanges[0].ChangeAmount == order.OrderAmount), cancellationToken), Times.Once);
            repositoryMock.Verify(r => r.AddAsync(It.Is<StockBookOrder>(s => s.Type == StockBookOrderType.ClientOrderCancel), cancellationToken), Times.Once);
        }
        [Test]
        public async Task AddStockBookOrderAsync_CalculatesTotalChangeAmount()
        {
            // Arrange
            var stockBookOrder = new StockBookOrder
            {
                StockBookChanges = new List<StockBookChange>
                {
                    new StockBookChange { ChangeAmount = 5 },
                    new StockBookChange { ChangeAmount = 10 }
                }
            };
            var queryable = new List<StockBookOrder>() { stockBookOrder }.AsQueryable().BuildMock();
            repositoryMock.Setup(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken)).ReturnsAsync(queryable);
            repositoryMock.Setup(r => r.AddAsync(stockBookOrder, cancellationToken)).ReturnsAsync(stockBookOrder);
            eventDispatcherMock.Setup(e => e.DispatchAsync(It.IsAny<BookStockAmountUpdatedEvent>(), cancellationToken)).Returns(Task.CompletedTask);
            // Act
            var result = await stockBookOrderService.AddStockBookOrderAsync(stockBookOrder, cancellationToken);
            // Assert
            Assert.That(result.TotalChangeAmount, Is.EqualTo(15));
            repositoryMock.Verify(r => r.AddAsync(stockBookOrder, cancellationToken), Times.Once);
            eventDispatcherMock.Verify(e => e.DispatchAsync(It.IsAny<BookStockAmountUpdatedEvent>(), cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetStockBookOrderByIdAsync_WhenOrderExists_ReturnsOrder()
        {
            // Arrange
            var stockBookOrder = new StockBookOrder
            {
                Id = 1,
                ClientId = "test-client",
                StockBookChanges = new List<StockBookChange> { new StockBookChange { BookId = 1, ChangeAmount = 5 } }
            };
            var queryable = new List<StockBookOrder> { stockBookOrder }.AsQueryable().BuildMock();
            repositoryMock.Setup(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken)).ReturnsAsync(queryable);
            // Act
            var result = await stockBookOrderService.GetStockBookOrderByIdAsync(1, cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(stockBookOrder));
            Assert.That(result?.ClientId, Is.EqualTo("test-client"));
            repositoryMock.Verify(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetStockBookAmountAsync_WhenCalled_ReturnsCorrectCount()
        {
            // Arrange
            var stockBookOrders = new List<StockBookOrder>
            {
                new StockBookOrder { Id = 1 },
                new StockBookOrder { Id = 2 }
            };
            var queryable = stockBookOrders.AsQueryable().BuildMock();
            repositoryMock.Setup(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken)).ReturnsAsync(queryable);
            // Act
            var result = await stockBookOrderService.GetStockBookAmountAsync(cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(2));
            repositoryMock.Verify(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetStockBookAmountAsync_WhenNoOrders_ReturnsZero()
        {
            // Arrange
            var stockBookOrders = new List<StockBookOrder>();
            var queryable = stockBookOrders.AsQueryable().BuildMock();
            repositoryMock.Setup(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken)).ReturnsAsync(queryable);
            // Act
            var result = await stockBookOrderService.GetStockBookAmountAsync(cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(0));
            repositoryMock.Verify(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetPaginatedStockBookOrdersAsync_ReturnsPaginatedOrders()
        {
            // Arrange
            var paginationRequest = new PaginationRequest { PageNumber = 1, PageSize = 2 };
            var stockBookOrders = new List<StockBookOrder>
            {
                new StockBookOrder { Id = 1 },
                new StockBookOrder { Id = 2 },
                new StockBookOrder { Id = 3 }
            };
            var queryable = stockBookOrders.AsQueryable().BuildMock();
            repositoryMock.Setup(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken)).ReturnsAsync(queryable);
            // Act
            var result = await stockBookOrderService.GetPaginatedStockBookOrdersAsync(paginationRequest, cancellationToken);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            repositoryMock.Verify(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken), Times.Once);
        }

        private Order CreateOrder()
        {
            return new Order
            {
                OrderAmount = 5,
                ClientId = "test-client",
                OrderBooks = new List<OrderBook>
                {
                    new OrderBook { BookId = 1, BookAmount = 5 }
                }
            };
        }
    }
}