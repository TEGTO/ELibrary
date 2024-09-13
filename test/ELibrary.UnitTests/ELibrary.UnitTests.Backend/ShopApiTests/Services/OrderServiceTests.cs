using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using MockQueryable.Moq;
using Moq;
using Shared.Dtos;
using ShopApi.Repositories;

namespace ShopApi.Services.Tests
{
    [TestFixture]
    internal class OrderServiceTests
    {
        private Mock<IShopDatabaseRepository> mockRepository;
        private OrderService orderService;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new Mock<IShopDatabaseRepository>();
            orderService = new OrderService(mockRepository.Object);
        }
        private static IQueryable<T> GetDbSetMock<T>(List<T> data) where T : class
        {
            return data.AsQueryable().BuildMock();
        }

        [Test]
        public async Task GetPaginatedAsync_ReturnsPaginatedOrders()
        {
            // Arrange
            var paginationRequest = new PaginationRequest { PageNumber = 1, PageSize = 2 };
            var orders = GetDbSetMock(new List<Order>
            {
                new Order { Id = 1, DeliveryAddress = "Address 1" },
                new Order { Id = 2, DeliveryAddress = "Address 2" }
            });

            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);
            // Act
            var result = await orderService.GetPaginatedAsync(paginationRequest, CancellationToken.None);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Id, Is.EqualTo(2));
        }
        [Test]
        public async Task GetOrdersByClientIdAsync_ReturnsOrdersForClient()
        {
            // Arrange
            var clientId = "test-client-id";
            var orders = GetDbSetMock(new List<Order>
            {
                new Order { Id = 1, ClientId = clientId, DeliveryAddress = "Address 1" },
                new Order { Id = 2, ClientId = clientId, DeliveryAddress = "Address 2" }
            });
            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);
            // Act
            var result = await orderService.GetOrdersByClientIdAsync(clientId, CancellationToken.None);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().ClientId, Is.EqualTo(clientId));
        }
        [Test]
        public async Task CreateOrderAsync_ReturnsCreatedOrder()
        {
            // Arrange
            var order = new Order { Id = 1, DeliveryAddress = "Test Address", Books = new List<Book> { new Book { Id = 1 } } };
            var bookIds = order.Books.Select(b => b.Id).ToList();
            mockRepository.Setup(r => r.CreateOrderAsync(It.IsAny<Order>(), It.IsAny<List<int>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(order);
            // Act
            var result = await orderService.CreateOrderAsync(order, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(1));
            mockRepository.Verify(r => r.CreateOrderAsync(order, bookIds, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task CheckOrderAsync_ReturnsTrue_WhenOrderExists()
        {
            // Arrange
            var clientId = "test-client-id";
            var orderId = 1;
            var orders = GetDbSetMock(new List<Order> { new Order { Id = orderId, ClientId = clientId } });
            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);
            // Act
            var result = await orderService.CheckOrderAsync(clientId, orderId, CancellationToken.None);
            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task CheckOrderAsync_ReturnsFalse_WhenOrderDoesNotExist()
        {
            // Arrange
            var clientId = "test-client-id";
            var orders = GetDbSetMock(new List<Order>());
            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);
            // Act
            var result = await orderService.CheckOrderAsync(clientId, 1, CancellationToken.None);
            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task UpdateOrderAsync_ReturnsUpdatedOrder()
        {
            // Arrange
            var order = new Order { Id = 1, DeliveryAddress = "Updated Address", Books = new List<Book> { new Book { Id = 1 } } };
            var bookIds = order.Books.Select(b => b.Id).ToList();
            mockRepository.Setup(r => r.UpdateOrderAsync(It.IsAny<Order>(), It.IsAny<List<int>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(order);
            // Act
            var result = await orderService.UpdateOrderAsync(order, CancellationToken.None);
            // Assert
            Assert.That(result.DeliveryAddress, Is.EqualTo("Updated Address"));
            mockRepository.Verify(r => r.UpdateOrderAsync(order, bookIds, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task DeleteOrderAsync_DeletesOrder()
        {
            // Arrange
            var order = new Order { Id = 1, DeliveryAddress = "Test Address" };
            var orders = GetDbSetMock(new List<Order> { order });
            mockRepository.Setup(r => r.GetQueryableAsync<Order>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(orders);
            // Act
            await orderService.DeleteOrderAsync(order.Id, CancellationToken.None);
            // Assert
            mockRepository.Verify(r => r.DeleteAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}