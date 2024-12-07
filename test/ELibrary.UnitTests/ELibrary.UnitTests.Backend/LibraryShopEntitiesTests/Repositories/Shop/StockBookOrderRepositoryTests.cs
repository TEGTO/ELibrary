using DatabaseControl.Repositories;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using MockQueryable.Moq;
using Moq;
using Pagination;

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

        [TestCase(1, "test-client", 1, 5, true, Description = "Order exists.")]
        [TestCase(99, "", 0, 0, false, Description = "Order does not exist.")]
        public async Task GetStockBookOrderByIdAsync_WithVariousInputs_ReturnsExpectedResults(
            int orderId, string clientId, int bookId, int changeAmount, bool exists)
        {
            // Arrange
            var stockBookOrder = new StockBookOrder
            {
                Id = orderId,
                ClientId = clientId,
                StockBookChanges = new List<StockBookChange> { new StockBookChange { BookId = bookId, ChangeAmount = changeAmount } }
            };
            var stockBookOrderList = exists ? new List<StockBookOrder> { stockBookOrder } : new List<StockBookOrder>();

            var queryable = stockBookOrderList.AsQueryable().BuildMock();

            repositoryMock.Setup(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken))
                          .ReturnsAsync(queryable);

            // Act
            var result = await stockBookOrderRepository.GetStockBookOrderByIdAsync(orderId, cancellationToken);

            // Assert
            if (exists)
            {
                Assert.IsNotNull(result);
                Assert.That(result?.Id, Is.EqualTo(orderId));
                Assert.That(result?.ClientId, Is.EqualTo(clientId));
            }
            else
            {
                Assert.IsNull(result);
            }
        }

        [TestCase(1, 2, 2, 3, Description = "Page size matches available orders.")]
        [TestCase(2, 2, 1, 2, Description = "Second page with remaining orders.")]
        [TestCase(3, 2, 0, null, Description = "Page number exceeds available orders.")]
        public async Task GetPaginatedStockBookOrdersAsync_WithVariousPagination_ReturnsExpectedResults(
            int pageNumber, int pageSize, int expectedCount, int? firstOrderId)
        {
            // Arrange
            var paginationRequest = new PaginationRequest { PageNumber = pageNumber, PageSize = pageSize };
            var stockBookOrders = new List<StockBookOrder>
            {
                new StockBookOrder { Id = 1, CreatedAt = DateTime.MinValue.AddDays(1)  },
                new StockBookOrder { Id = 2, CreatedAt = DateTime.MinValue},
                new StockBookOrder { Id = 3, CreatedAt = DateTime.MaxValue}
            };
            var queryable = stockBookOrders.AsQueryable().BuildMock();

            repositoryMock.Setup(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken))
                          .ReturnsAsync(queryable);

            // Act
            var result = await stockBookOrderRepository.GetPaginatedStockBookOrdersAsync(paginationRequest, cancellationToken);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(expectedCount));
            if (firstOrderId != null)
            {
                Assert.That(result.First().Id, Is.EqualTo(firstOrderId));
            }
        }

        [TestCase(3, Description = "Orders exist.")]
        [TestCase(0, Description = "No orders exist.")]
        public async Task GetStockBookOrderAmountAsync_WithVariousOrders_ReturnsExpectedCount(int orderCount)
        {
            // Arrange
            var stockBookOrders = Enumerable.Range(1, orderCount)
                                             .Select(id => new StockBookOrder { Id = id })
                                             .ToList();
            var queryable = stockBookOrders.AsQueryable().BuildMock();

            repositoryMock.Setup(r => r.GetQueryableAsync<StockBookOrder>(cancellationToken))
                          .ReturnsAsync(queryable);

            // Act
            var result = await stockBookOrderRepository.GetStockBookOrderAmountAsync(cancellationToken);

            // Assert
            Assert.That(result, Is.EqualTo(orderCount));
        }

        [TestCase(1, "Test Address")]
        [TestCase(2, "Another Address")]
        public async Task AddStockBookOrderAsync_WithVariousOrders_AddsOrder(int orderId, string address)
        {
            // Arrange
            var stockBookOrder = new StockBookOrder { Id = orderId, ClientId = address };
            var newStockBookOrder = new StockBookOrder { Id = orderId, ClientId = address };

            repositoryMock.Setup(r => r.AddAsync(stockBookOrder, cancellationToken))
                          .ReturnsAsync(newStockBookOrder);

            // Act
            var result = await stockBookOrderRepository.AddStockBookOrderAsync(stockBookOrder, cancellationToken);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(orderId));

            repositoryMock.Verify(r => r.AddAsync(stockBookOrder, cancellationToken), Times.Once);
        }
    }
}