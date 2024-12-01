using DatabaseControl.Repositories;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using LibraryShopEntities.Filters;
using MockQueryable.Moq;
using Moq;

namespace LibraryShopEntities.Repositories.Shop.Tests
{
    [TestFixture]
    internal class OrderRepositoryTests
    {
        private Mock<IDatabaseRepository<ShopDbContext>> mockRepository;
        private OrderRepository orderRepository;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new Mock<IDatabaseRepository<ShopDbContext>>();

            orderRepository = new OrderRepository(mockRepository.Object);
        }

        private static IQueryable<T> GetDbSetMock<T>(List<T> data) where T : class
        {
            return data.AsQueryable().BuildMock();
        }

        [TestCase(1, true, "Address 1", Description = "Order exists.")]
        [TestCase(99, false, null, Description = "Order does not exist.")]
        public async Task GetOrderByIdAsync_VariousCases_ReturnsExpectedResults(int orderId, bool exists, string? expectedAddress)
        {
            // Arrange
            var orders = GetDbSetMock(new List<Order>
            {
                new Order { Id = 1, DeliveryAddress = "Address 1" }
            });

            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);

            // Act
            var result = await orderRepository.GetOrderByIdAsync(orderId, CancellationToken.None);

            // Assert
            if (exists)
            {
                Assert.IsNotNull(result);
                Assert.That(result.Id, Is.EqualTo(orderId));
                Assert.That(result.DeliveryAddress, Is.EqualTo(expectedAddress));
            }
            else
            {
                Assert.IsNull(result);
            }
        }

        [TestCase(1, 2, "", 2, Description = "Valid pagination, returns all orders.")]
        [TestCase(2, 1, "", 1, Description = "Page number exceeds available items.")]
        [TestCase(1, 2, "Client1", 2, Description = "Get orders of 'Client1'.")]
        [TestCase(2, 2, "Client0", 0, Description = "Get zero orders because client with this id doesn't exist.")]
        public async Task GetPaginatedOrdersAsync_VariousFilters_ReturnsExpectedResults(int pageNumber, int pageSize, string clientId, int expectedCount)
        {
            // Arrange
            var filter = new GetOrdersFilter { PageNumber = pageNumber, PageSize = pageSize };
            var orders = GetDbSetMock(new List<Order>
            {
                new Order { Id = 1, DeliveryAddress = "Address 1", ClientId = "Client1" },
                new Order { Id = 2, DeliveryAddress = "Address 2", ClientId = "Client2" }
            });

            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);

            // Act
            var result = await orderRepository.GetPaginatedOrdersAsync(filter, CancellationToken.None);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(expectedCount));
        }

        [TestCase("test-client-id", 2, Description = "Filter matches orders.")]
        [TestCase("non-existent-client", 0, Description = "Filter does not match any orders.")]
        public async Task GetOrderCountAsync_VariousFilters_ReturnsExpectedCount(string clientId, int expectedCount)
        {
            // Arrange
            var filter = new GetOrdersFilter { ClientId = clientId };
            var orders = GetDbSetMock(new List<Order>
            {
                new Order { Id = 1, ClientId = "test-client-id" },
                new Order { Id = 2, ClientId = "test-client-id" }
            });

            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);

            // Act
            var result = await orderRepository.GetOrderCountAsync(filter, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(expectedCount));
        }

        [TestCase(1, "Test Address", 1, 1, 10, Description = "Single OrderBook.")]
        [TestCase(2, "Another Address", 2, 3, 15, Description = "Another OrderBook.")]
        public async Task AddOrderAsync_WithVariousOrders_ReturnsCreatedOrder(
           int orderId, string address, int bookId, int amount, decimal price)
        {
            // Arrange
            var order = new Order
            {
                Id = orderId,
                DeliveryAddress = address,
                OrderBooks = new List<OrderBook>
                {
                    new OrderBook { BookId = bookId, BookAmount = amount, BookPrice = price }
                }
            };

            mockRepository.Setup(r => r.AddAsync(order, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(order);

            // Act
            var result = await orderRepository.AddOrderAsync(order, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(orderId));
            Assert.That(result.DeliveryAddress, Is.EqualTo(address));
            Assert.That(result.OrderBooks[0].BookId, Is.EqualTo(bookId));

            mockRepository.Verify(r => r.AddAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCase(1, "Updated Address")]
        [TestCase(2, "Another Updated Address")]
        public async Task UpdateOrderAsync_WithVariousUpdates_ReturnsUpdatedOrder(int orderId, string updatedAddress)
        {
            // Arrange
            var order = new Order { Id = orderId, DeliveryAddress = updatedAddress };

            mockRepository.Setup(r => r.UpdateAsync(order, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(order);

            // Act
            var result = await orderRepository.UpdateOrderAsync(order, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(orderId));
            Assert.That(result.DeliveryAddress, Is.EqualTo(updatedAddress));

            mockRepository.Verify(r => r.UpdateAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCase(1, "Test Address")]
        [TestCase(2, "Another Address")]
        public async Task DeleteOrderAsync_WithVariousOrders_DeletesOrder(int orderId, string address)
        {
            // Arrange
            var order = new Order { Id = orderId, DeliveryAddress = address };

            // Act
            await orderRepository.DeleteOrderAsync(order, CancellationToken.None);

            // Assert
            mockRepository.Verify(r => r.DeleteAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}