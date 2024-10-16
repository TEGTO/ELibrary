﻿using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
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
        public async Task GetPaginatedOrdersAsync_WithClient_ReturnsMappedOrders()
        {
            // Arrange
            var client = new Client { Id = "client123" };
            var filter = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            var orders = new List<Order> { new Order { Id = 1 }, new Order { Id = 2 } };
            var orderResponses = new List<OrderResponse> { new OrderResponse { Id = 1 }, new OrderResponse { Id = 2 } };
            orderServiceMock.Setup(service => service.GetPaginatedOrdersAsync(filter, It.IsAny<CancellationToken>()))
                .ReturnsAsync(orders);
            mapperMock.Setup(mapper => mapper.Map<OrderResponse>(It.IsAny<Order>()))
                .Returns((Order order) => new OrderResponse { Id = order.Id });
            // Act
            var result = await manager.GetPaginatedOrdersAsync(filter, client, CancellationToken.None);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(orderResponses.Count));
            Assert.That(filter.ClientId, Is.EqualTo(client.Id));
            Assert.That(result.First().Id, Is.EqualTo(orderResponses.First().Id));
        }
        [Test]
        public async Task GetPaginatedOrdersAsync_WithoutClient_ReturnsMappedOrders()
        {
            // Arrange
            var filter = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            var orders = new List<Order> { new Order { Id = 1 }, new Order { Id = 2 } };
            var orderResponses = new List<OrderResponse> { new OrderResponse { Id = 1 }, new OrderResponse { Id = 2 } };
            orderServiceMock.Setup(service => service.GetPaginatedOrdersAsync(filter, It.IsAny<CancellationToken>()))
                .ReturnsAsync(orders);
            mapperMock.Setup(mapper => mapper.Map<OrderResponse>(It.IsAny<Order>()))
                .Returns((Order order) => new OrderResponse { Id = order.Id });
            // Act
            var result = await manager.GetPaginatedOrdersAsync(filter, CancellationToken.None);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(orderResponses.Count));
            Assert.That(result.First().Id, Is.EqualTo(orderResponses.First().Id));
        }
        [Test]
        public async Task GetOrderAmountAsync_WithClient_ReturnsOrderAmount()
        {
            // Arrange
            var client = new Client { Id = "client123" };
            var filter = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            var expectedAmount = 5;
            orderServiceMock.Setup(service => service.GetOrderAmountAsync(filter, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAmount);
            // Act
            var result = await manager.GetOrderAmountAsync(filter, client, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(expectedAmount));
            Assert.That(filter.ClientId, Is.EqualTo(client.Id));
            orderServiceMock.Verify(service => service.GetOrderAmountAsync(filter, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task GetOrderAmountAsync_WithoutClient_ReturnsOrderAmount()
        {
            // Arrange
            var expectedAmount = 10;
            var filter = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            orderServiceMock.Setup(service => service.GetOrderAmountAsync(filter, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAmount);
            // Act
            var result = await manager.GetOrderAmountAsync(filter, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(expectedAmount));
            orderServiceMock.Verify(service => service.GetOrderAmountAsync(filter, It.IsAny<CancellationToken>()), Times.Once);
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
            stockBookOrderServiceMock.Verify(x => x.AddStockBookOrderAsyncFromOrderAsync(order, StockBookOrderType.ClientOrder, It.IsAny<CancellationToken>()), Times.Once);
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
            stockBookOrderServiceMock.Verify(x => x.AddStockBookOrderAsyncFromCanceledOrderAsync(canceledOrder, StockBookOrderType.ClientOrderCancel, It.IsAny<CancellationToken>()), Times.Once);
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
            var order = new Order { Id = orderId, ClientId = client.Id, OrderStatus = OrderStatus.Completed };
            orderServiceMock.Setup(service => service.GetOrderByIdAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await manager.CancelOrderAsync(orderId, client, CancellationToken.None));
        }
        [Test]
        public async Task GetOrderByIdAsync_ValidRequest_ReturnsMappedOrder()
        {
            // Arrange
            var order = new Order { Id = 1 };
            orderServiceMock.Setup(service => service.GetOrderByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            mapperMock.Setup(mapper => mapper.Map<OrderResponse>(It.IsAny<Order>()))
                .Returns((Order order) => new OrderResponse { Id = order.Id });
            // Act
            var result = await manager.GetOrderByIdAsync(1, CancellationToken.None);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(order.Id));
            Assert.That(result.TotalPrice, Is.EqualTo(order.TotalPrice));
        }
        [Test]
        public async Task GetOrderByIdAsync_InvalidRequest_ReturnsNullOrder()
        {
            // Arrange
            orderServiceMock.Setup(service => service.GetOrderByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()));
            // Act
            var result = await manager.GetOrderByIdAsync(2, CancellationToken.None);
            // Assert
            Assert.That(result, Is.Null);
        }
        [Test]
        public async Task UpdateOrderAsync_ValidRequest_ReturnsMappedUpdatedOrder()
        {
            // Arrange
            var updateRequest = new ManagerUpdateOrderRequest { Id = 1, DeliveryAddress = "New Address" };
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