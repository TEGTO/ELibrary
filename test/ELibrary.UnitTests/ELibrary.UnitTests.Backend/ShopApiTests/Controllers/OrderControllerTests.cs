using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopApi.Domain.Dtos.Order;
using ShopApi.Services;
using System.Security.Claims;

namespace ShopApi.Controllers.Tests
{
    [TestFixture]
    internal class OrderControllerTests
    {
        private Mock<IMapper> mockMapper;
        private Mock<IOrderService> mockOrderService;
        private Mock<IClientService> mockClientService;
        private OrderController orderController;

        [SetUp]
        public void SetUp()
        {
            mockMapper = new Mock<IMapper>();
            mockOrderService = new Mock<IOrderService>();
            mockClientService = new Mock<IClientService>();

            orderController = new OrderController(mockMapper.Object, mockOrderService.Object, mockClientService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
            }, "mock"));

            orderController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Test]
        public async Task GetOrders_ReturnsOrders_WhenClientExists()
        {
            // Arrange
            var client = new Client { Id = "test-client-id" };
            var orders = new List<Order>
            {
                new Order { Id = 1, ClientId = "test-client-id", DeliveryAddress = "Address 1" }
            };
            var orderResponses = new List<OrderResponse>
            {
                new OrderResponse { Id = 1, DeliveryAddress = "Address 1" }
            };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            mockOrderService.Setup(os => os.GetOrdersByClientIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(orders);
            mockMapper.Setup(m => m.Map<OrderResponse>(It.IsAny<Order>())).Returns(orderResponses.First());
            // Act
            var result = await orderController.GetOrders(CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(orderResponses));
        }
        [Test]
        public async Task GetOrders_ReturnsBadRequest_WhenClientNotFound()
        {
            // Arrange
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client?)null);
            // Act
            var result = await orderController.GetOrders(CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Client is not found!"));
        }
        [Test]
        public async Task CreateOrder_ReturnsCreated_WhenClientExists()
        {
            // Arrange
            var client = new Client { Id = "test-client-id" };
            var createRequest = new CreateOrderRequest { DeliveryAddress = "Test Address" };
            var order = new Order { ClientId = "test-client-id" };
            var orderResponse = new OrderResponse { Id = 1 };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockMapper.Setup(m => m.Map<Order>(createRequest)).Returns(order);
            mockOrderService.Setup(os => os.CreateOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            mockMapper.Setup(m => m.Map<OrderResponse>(order)).Returns(orderResponse);
            // Act
            var result = await orderController.CreateOrder(createRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<CreatedResult>(result.Result);
            var createdResult = result.Result as CreatedResult;
            Assert.That(createdResult?.Value, Is.EqualTo(orderResponse));
        }
        [Test]
        public async Task CreateOrder_ReturnsBadRequest_WhenClientNotFound()
        {
            // Arrange
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client?)null);
            // Act
            var result = await orderController.CreateOrder(new CreateOrderRequest(), CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Client is not found!"));
        }
        [Test]
        public async Task UpdateOrder_ReturnsOk_WhenOrderExists()
        {
            // Arrange
            var client = new Client { Id = "test-client-id" };
            var updateRequest = new PatchOrderRequest { Id = 1, DeliveryAddress = "Updated Address" };
            var order = new Order { Id = 1, ClientId = "test-client-id", DeliveryAddress = "Updated Address" };
            var expectedResponse = new OrderResponse { Id = 1, DeliveryAddress = "Updated Address" };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockOrderService.Setup(os => os.CheckOrderAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            mockOrderService.Setup(os => os.GetOrderByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            mockMapper.Setup(m => m.Map<Order>(updateRequest)).Returns(order);
            mockMapper.Setup(m => m.Map<OrderResponse>(It.IsAny<Order>())).Returns(expectedResponse);
            mockOrderService.Setup(os => os.UpdateOrderAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            // Act
            var result = await orderController.UpdateOrder(updateRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedResponse));
        }
        [Test]
        public async Task UpdateOrder_ReturnsBadRequest_WhenOrderNotFound()
        {
            // Arrange
            var client = new Client { Id = "test-client-id" };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockOrderService.Setup(os => os.CheckOrderAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            // Act
            var result = await orderController.UpdateOrder(new PatchOrderRequest { Id = 1 }, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Order is not found!"));
        }
        [Test]
        public async Task DeleteOrder_ReturnsOk_WhenOrderExists()
        {
            // Arrange
            var client = new Client { Id = "test-client-id" };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockOrderService.Setup(os => os.CheckOrderAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            // Act
            var result = await orderController.DeleteOrder(1, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task DeleteOrder_ReturnsBadRequest_WhenOrderNotFound()
        {
            // Arrange
            var client = new Client { Id = "test-client-id" };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockOrderService.Setup(os => os.CheckOrderAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            // Act
            var result = await orderController.DeleteOrder(1, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Order is not found!"));
        }
    }
}