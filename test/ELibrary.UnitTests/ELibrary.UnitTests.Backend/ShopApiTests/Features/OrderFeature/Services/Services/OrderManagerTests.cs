using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using Shared.Dtos;
using ShopApi.Features.OrderFeature.Dtos;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Features.StockBookOrderFeature.Services;

namespace ShopApiTests.Features.OrderFeature.Services.Services
{
    [TestFixture]
    internal class OrderManagerTests
    {
        private Mock<IOrderService> orderServiceMock;
        private Mock<IMapper> mapperMock;
        private Mock<IStockBookOrderService> stockBookOrderServiceMock;
        private OrderManager manager;

        [SetUp]
        public void SetUp()
        {
            orderServiceMock = new Mock<IOrderService>();
            mapperMock = new Mock<IMapper>();
            stockBookOrderServiceMock = new Mock<IStockBookOrderService>();
            manager = new OrderManager(mapperMock.Object, orderServiceMock.Object, stockBookOrderServiceMock.Object);
        }

        [Test]
        public async Task GetOrdersByClientIdAsync_ValidClientId_ReturnsMappedOrders()
        {
            // Arrange
            var clientId = "client123";
            var paginationRequest = new PaginationRequest { PageNumber = 1, PageSize = 10 };
            var orders = new List<Order> { new Order { Id = 1 }, new Order { Id = 2 } };
            var orderResponses = new List<OrderResponse> { new OrderResponse { Id = 1 }, new OrderResponse { Id = 2 } };
            orderServiceMock.Setup(service => service.GetPaginatedOrdersAsync(clientId, paginationRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(orders);
            mapperMock.Setup(mapper => mapper.Map<OrderResponse>(It.IsAny<Order>()))
                .Returns((Order order) => new OrderResponse { Id = order.Id });
            // Act
            var result = await manager.GetPaginatedOrdersAsync(clientId, paginationRequest, CancellationToken.None);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(orderResponses.Count));
            Assert.That(result.First().Id, Is.EqualTo(orderResponses.First().Id));
        }
        [Test]
        public async Task GetOrderAmountAsync_WithClientId_ReturnsOrderAmount()
        {
            // Arrange
            var clientId = "test-client-id";
            var expectedAmount = 5;
            orderServiceMock.Setup(service => service.GetOrderAmountAsync(clientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAmount);
            // Act
            var result = await manager.GetOrderAmountAsync(clientId, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(expectedAmount));
            orderServiceMock.Verify(service => service.GetOrderAmountAsync(clientId, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task GetOrderAmountAsync_WithoutClientId_ReturnsOrderAmount()
        {
            // Arrange
            var expectedAmount = 10;
            orderServiceMock.Setup(service => service.GetOrderAmountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAmount);
            // Act
            var result = await manager.GetOrderAmountAsync(CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(expectedAmount));
            orderServiceMock.Verify(service => service.GetOrderAmountAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task CreateOrderAsync_ValidRequest_ReturnsMappedOrder()
        {
            // Arrange
            var client = new Client { Id = "client123" };
            var createOrderRequest = new CreateOrderRequest { DeliveryAddress = "123 Street" };
            var order = new Order { ClientId = client.Id };
            var createdOrder = new Order { Id = 1, ClientId = client.Id };
            var orderResponse = new OrderResponse { Id = 1 };
            mapperMock.Setup(mapper => mapper.Map<Order>(createOrderRequest)).Returns(order);
            orderServiceMock.Setup(service => service.CreateOrderAsync(order, It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdOrder);
            mapperMock.Setup(mapper => mapper.Map<OrderResponse>(createdOrder)).Returns(orderResponse);
            // Act
            var result = await manager.CreateOrderAsync(createOrderRequest, client, CancellationToken.None);
            // Assert
            Assert.That(result.Id, Is.EqualTo(orderResponse.Id));
            stockBookOrderServiceMock.Verify(x => x.AddStockBookOrderAsyncFromOrderAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task UpdateOrderAsync_ValidRequest_ReturnsUpdatedOrder()
        {
            // Arrange
            var client = new Client { Id = "client123" };
            var updateRequest = new ClientUpdateOrderRequest { Id = 1, DeliveryAddress = "New Address" };
            var order = new Order { Id = 1, ClientId = client.Id };
            var updatedOrder = new Order { Id = 1, DeliveryAddress = "New Address" };
            var orderResponse = new OrderResponse { Id = 1, DeliveryAddress = "New Address" };

            orderServiceMock.Setup(service => service.GetOrderByIdAsync(updateRequest.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            orderServiceMock.Setup(service => service.UpdateOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedOrder);
            mapperMock.Setup(mapper => mapper.Map<OrderResponse>(updatedOrder)).Returns(orderResponse);
            // Act
            var result = await manager.UpdateOrderAsync(updateRequest, client, CancellationToken.None);
            // Assert
            Assert.That(result.Id, Is.EqualTo(orderResponse.Id));
            Assert.That(result.DeliveryAddress, Is.EqualTo(orderResponse.DeliveryAddress));
        }
        [Test]
        public void UpdateOrderAsync_OrderNotFoundOrClientMismatch_ThrowsInvalidOperationException()
        {
            // Arrange
            var client = new Client { Id = "client123" };
            var updateRequest = new ClientUpdateOrderRequest { Id = 1 };
            orderServiceMock.Setup(service => service.GetOrderByIdAsync(updateRequest.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order)null);
            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await manager.UpdateOrderAsync(updateRequest, client, CancellationToken.None));
        }
        [Test]
        public async Task CancelOrderAsync_ValidOrder_ReturnsCanceledOrder()
        {
            // Arrange
            var client = new Client { Id = "client123" };
            var orderId = 1;
            var order = new Order { Id = orderId, ClientId = client.Id, OrderStatus = OrderStatus.InProcessing };
            var canceledOrder = new Order { Id = orderId, ClientId = client.Id, OrderStatus = OrderStatus.Canceled };
            var orderResponse = new OrderResponse { Id = orderId, OrderStatus = OrderStatus.Canceled };
            orderServiceMock.Setup(service => service.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            orderServiceMock.Setup(service => service.UpdateOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(canceledOrder);
            mapperMock.Setup(mapper => mapper.Map<OrderResponse>(canceledOrder)).Returns(orderResponse);
            // Act
            var result = await manager.CancelOrderAsync(orderId, client, CancellationToken.None);
            // Assert
            Assert.That(result.Id, Is.EqualTo(orderResponse.Id));
            Assert.That(OrderStatus.Canceled, Is.EqualTo(orderResponse.OrderStatus));
            stockBookOrderServiceMock.Verify(x => x.AddStockBookOrderAsyncFromCanceledOrderAsync(canceledOrder, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void CancelOrderAsync_OrderNotFoundOrClientMismatch_ThrowsInvalidOperationException()
        {
            // Arrange
            var client = new Client { Id = "client123" };
            var orderId = 1;
            orderServiceMock.Setup(service => service.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order)null);
            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await manager.CancelOrderAsync(orderId, client, CancellationToken.None));
        }
        [Test]
        public void CancelOrderAsync_InvalidOrderStatus_ThrowsInvalidOperationException()
        {
            // Arrange
            var client = new Client { Id = "client123" };
            var orderId = 1;
            var order = new Order { Id = orderId, ClientId = client.Id, OrderStatus = OrderStatus.Delivered };
            orderServiceMock.Setup(service => service.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await manager.CancelOrderAsync(orderId, client, CancellationToken.None));
        }
        [Test]
        public async Task GetPaginatedOrdersAsync_ValidRequest_ReturnsMappedOrders()
        {
            // Arrange
            var paginationRequest = new PaginationRequest { PageNumber = 1, PageSize = 10 };
            var orders = new List<Order> { new Order { Id = 1 }, new Order { Id = 2 } };
            var orderResponses = new List<OrderResponse> { new OrderResponse { Id = 1 }, new OrderResponse { Id = 2 } };
            orderServiceMock.Setup(service => service.GetPaginatedOrdersAsync(paginationRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(orders);
            mapperMock.Setup(mapper => mapper.Map<OrderResponse>(It.IsAny<Order>()))
                .Returns((Order order) => new OrderResponse { Id = order.Id });
            // Act
            var result = await manager.GetPaginatedOrdersAsync(paginationRequest, CancellationToken.None);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(orderResponses.Count));
            Assert.That(result.First().Id, Is.EqualTo(orderResponses.First().Id));
            Assert.That(result.Last().Id, Is.EqualTo(orderResponses.Last().Id));
        }
        [Test]
        public async Task UpdateOrderAsync_ValidRequest_ReturnsMappedUpdatedOrder()
        {
            // Arrange
            var updateRequest = new ManagerUpdateOrderRequest { Id = 1, DeliveryAddress = "New Address", OrderStatus = OrderStatus.Packed };
            var order = new Order { Id = 1, DeliveryAddress = "Old Address" };
            var updatedOrder = new Order { Id = 1, DeliveryAddress = "New Address" };
            var orderResponse = new OrderResponse { Id = 1, DeliveryAddress = "New Address" };
            mapperMock.Setup(mapper => mapper.Map<Order>(updateRequest)).Returns(order);
            orderServiceMock.Setup(service => service.UpdateOrderAsync(order, It.IsAny<CancellationToken>())).ReturnsAsync(updatedOrder);
            mapperMock.Setup(mapper => mapper.Map<OrderResponse>(updatedOrder)).Returns(orderResponse);
            // Act
            var result = await manager.UpdateOrderAsync(updateRequest, CancellationToken.None);
            // Assert
            Assert.That(result.Id, Is.EqualTo(orderResponse.Id));
            Assert.That(result.DeliveryAddress, Is.EqualTo(orderResponse.DeliveryAddress));
        }
        [Test]
        public async Task CancelOrderAsync_ValidOrderId_ReturnsMappedCanceledOrder()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { Id = orderId, OrderStatus = OrderStatus.InProcessing };
            var canceledOrder = new Order { Id = orderId, OrderStatus = OrderStatus.Canceled };
            var orderResponse = new OrderResponse { Id = orderId, OrderStatus = OrderStatus.Canceled };
            orderServiceMock.Setup(service => service.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>())).ReturnsAsync(order);
            orderServiceMock.Setup(service => service.UpdateOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>())).ReturnsAsync(canceledOrder);
            mapperMock.Setup(mapper => mapper.Map<OrderResponse>(canceledOrder)).Returns(orderResponse);
            // Act
            var result = await manager.CancelOrderAsync(orderId, CancellationToken.None);
            // Assert
            Assert.That(result.Id, Is.EqualTo(orderResponse.Id));
            Assert.That(OrderStatus.Canceled, Is.EqualTo(orderResponse.OrderStatus));
        }
        [Test]
        public void CancelOrderAsync_OrderNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var orderId = 1;
            orderServiceMock.Setup(service => service.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order)null);
            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await manager.CancelOrderAsync(orderId, CancellationToken.None));
        }
    }
}