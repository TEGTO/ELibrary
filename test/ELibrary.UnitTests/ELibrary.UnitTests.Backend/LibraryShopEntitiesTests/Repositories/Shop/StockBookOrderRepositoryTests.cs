using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using MockQueryable.Moq;
using Moq;
using Pagination;
using Shared.Repositories;

namespace LibraryShopEntities.Repositories.Shop.Tests
{
    [TestFixture]
    internal class StockBookOrderRepositoryTests
    {
        private Mock<IDatabaseRepository<ShopDbContext>> repositoryMock;
        private StockBookOrderRepository stockBookOrderRepository;
        private CancellationToken cancellationToken;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IDatabaseRepository<ShopDbContext>>();
            stockBookOrderRepository = new StockBookOrderRepository(repositoryMock.Object);
            cancellationToken = CancellationToken.None;
        }

        [Test]
        public async Task AddStockBookOrderAsync_AddsStockBookOrder()
        {
            // Arrange
            var stockBookOrder = new StockBookOrder();
            var newStockBookOrder = new StockBookOrder();
            var queryable = new List<StockBookOrder>() { stockBookOrder }.AsQueryable().BuildMock();
            repositoryMock.Setup(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken)).ReturnsAsync(queryable);
            repositoryMock.Setup(r => r.AddAsync(stockBookOrder, cancellationToken)).ReturnsAsync(newStockBookOrder);
            // Act
            var result = await stockBookOrderRepository.AddStockBookOrderAsync(stockBookOrder, cancellationToken);
            // Assert
            Assert.That(result.Id, Is.EqualTo(newStockBookOrder.Id));
            repositoryMock.Verify(r => r.AddAsync(stockBookOrder, cancellationToken), Times.Once);
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
            var result = await stockBookOrderRepository.GetStockBookOrderByIdAsync(1, cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(stockBookOrder));
            Assert.That(result?.ClientId, Is.EqualTo("test-client"));
            repositoryMock.Verify(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetStockBookOrderAmountAsync_WhenCalled_ReturnsCorrectCount()
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
            var result = await stockBookOrderRepository.GetStockBookOrderAmountAsync(cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(2));
            repositoryMock.Verify(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetStockBookOrderAmountAsync_WhenNoOrders_ReturnsZero()
        {
            // Arrange
            var stockBookOrders = new List<StockBookOrder>();
            var queryable = stockBookOrders.AsQueryable().BuildMock();
            repositoryMock.Setup(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken)).ReturnsAsync(queryable);
            // Act
            var result = await stockBookOrderRepository.GetStockBookOrderAmountAsync(cancellationToken);
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
            var result = await stockBookOrderRepository.GetPaginatedStockBookOrdersAsync(paginationRequest, cancellationToken);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            repositoryMock.Verify(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken), Times.Once);
        }
    }
}