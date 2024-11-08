using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using LibraryShopEntities.Filters;
using MockQueryable.Moq;
using Moq;
using Shared.Repositories;

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
        private IQueryable<T> GetDbSetMock<T>(List<T> data) where T : class
        {
            return data.AsQueryable().BuildMock();
        }

        [Test]
        public async Task GetOrderByIdAsync_OrderExists_ReturnsOrder()
        {
            // Arrange
            var orderId = 1;
            var orders = GetDbSetMock(new List<Order>
            {
                new Order { Id = orderId, DeliveryAddress = "Address 1" }
            });
            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);
            // Act
            var result = await orderRepository.GetOrderByIdAsync(orderId, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(orderId));
            Assert.That(result.DeliveryAddress, Is.EqualTo("Address 1"));
        }
        [Test]
        public async Task GetOrderByIdAsync_OrderDoesNotExist_ReturnsNull()
        {
            // Arrange
            var orderId = 1;
            var orders = GetDbSetMock(new List<Order>());
            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);
            // Act
            var result = await orderRepository.GetOrderByIdAsync(orderId, CancellationToken.None);
            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task GetPaginatedOrdersAsync_ValidRequest_ReturnsPaginatedOrders()
        {
            // Arrange
            var filter = new GetOrdersFilter { PageNumber = 1, PageSize = 2 };
            var orders = GetDbSetMock(new List<Order>
            {
                new Order { Id = 1, DeliveryAddress = "Address 1" },
                new Order { Id = 2, DeliveryAddress = "Address 2" }
            });
            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);
            // Act
            var result = await orderRepository.GetPaginatedOrdersAsync(filter, CancellationToken.None);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Id, Is.EqualTo(1));
            Assert.That(result.Last().Id, Is.EqualTo(2));
        }
        [Test]
        public async Task GetOrderCountAsync_ValidData_ReturnsOrderCount()
        {
            // Arrange
            var filter = new GetOrdersFilter { ClientId = "test-client-id" };
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
            Assert.That(result, Is.EqualTo(2));
        }
        [Test]
        public async Task AddOrderAsync_ReturnsCreatedOrder()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                DeliveryAddress = "Test Address",
                OrderBooks = new List<OrderBook>
                {
                    new OrderBook { BookId = 1, BookAmount = 1, BookPrice = 10 }
                }
            };

            mockRepository.Setup(r => r.AddAsync(order, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(order);
            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                         .ReturnsAsync(GetDbSetMock(new List<Order> { order }));
            // Act
            var result = await orderRepository.AddOrderAsync(order, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(1));
            mockRepository.Verify(r => r.AddAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task UpdateOrderAsync_ReturnsUpdatedOrder()
        {
            // Arrange
            var order = new Order { Id = 1, DeliveryAddress = "Updated Address" };
            var orders = GetDbSetMock(new List<Order> { order });

            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);
            mockRepository.Setup(r => r.UpdateAsync(order, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(order);
            // Act
            var result = await orderRepository.UpdateOrderAsync(order, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.DeliveryAddress, Is.EqualTo("Updated Address"));
            mockRepository.Verify(r => r.UpdateAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task DeleteOrderAsync_DeletesOrder()
        {
            // Arrange
            var order = new Order { Id = 1, DeliveryAddress = "Test Address" };
            // Act
            await orderRepository.DeleteOrderAsync(order, CancellationToken.None);
            // Assert
            mockRepository.Verify(r => r.DeleteAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}