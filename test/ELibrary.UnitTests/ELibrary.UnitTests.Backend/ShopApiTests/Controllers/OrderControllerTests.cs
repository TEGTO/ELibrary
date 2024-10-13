using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopApi.Features.ClientFeature.Services;
using ShopApi.Features.OrderFeature.Dtos;
using ShopApi.Features.OrderFeature.Services;
using System.Security.Claims;

namespace ShopApi.Controllers.Tests
{
    [TestFixture]
    internal class OrderControllerTests
    {
        private Mock<IOrderManager> mockOrderManager;
        private Mock<IClientService> mockClientService;
        private OrderController orderController;

        [SetUp]
        public void SetUp()
        {
            mockOrderManager = new Mock<IOrderManager>();
            mockClientService = new Mock<IClientService>();

            orderController = new OrderController(
                mockOrderManager.Object,
                mockClientService.Object
            );

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
            var filter = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            var orders = new List<OrderResponse>
            {
                new OrderResponse { Id = 1, DeliveryAddress = "Address 1" }
            };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockOrderManager.Setup(och => och.GetPaginatedOrdersAsync(filter, client, It.IsAny<CancellationToken>()))
                .ReturnsAsync(orders);
            // Act
            var result = await orderController.GetOrders(filter, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(orders));
        }
        [Test]
        public async Task GetOrders_ReturnsBadRequest_WhenClientNotFound()
        {
            // Arrange
            var filter = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client?)null);
            // Act
            var result = await orderController.GetOrders(filter, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Client is not found!"));
        }
        [Test]
        public async Task GetOrderAmount_ReturnsAmount_WhenClientExists()
        {
            // Arrange
            var filter = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            var client = new Client { Id = "test-client-id" };
            var expectedAmount = 5;
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockOrderManager.Setup(om => om.GetOrderAmountAsync(filter, client, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAmount);
            // Act
            var result = await orderController.GetOrderAmount(filter, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedAmount));
        }
        [Test]
        public async Task GetOrderAmount_ReturnsBadRequest_WhenClientNotFound()
        {
            // Arrange
            var filter = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client?)null);
            // Act
            var result = await orderController.GetOrderAmount(filter, CancellationToken.None);
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
            var orderResponse = new OrderResponse { Id = 1 };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockOrderManager.Setup(och => och.CreateOrderAsync(It.IsAny<CreateOrderRequest>(), client, It.IsAny<CancellationToken>()))
                .ReturnsAsync(orderResponse);
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
            var updateRequest = new ClientUpdateOrderRequest { Id = 1, DeliveryAddress = "Updated Address" };
            var orderResponse = new OrderResponse { Id = 1, DeliveryAddress = "Updated Address" };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockOrderManager.Setup(och => och.UpdateOrderAsync(It.IsAny<ClientUpdateOrderRequest>(), client, It.IsAny<CancellationToken>()))
                .ReturnsAsync(orderResponse);
            // Act
            var result = await orderController.UpdateOrder(updateRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(orderResponse));
        }
        [Test]
        public async Task UpdateOrder_ReturnsBadRequest_WhenClientNotFound()
        {
            // Arrange
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client?)null);
            // Act
            var result = await orderController.UpdateOrder(new ClientUpdateOrderRequest { Id = 1 }, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Client is not found!"));
        }
        [Test]
        public async Task CancelOrder_ReturnsOk_WhenOrderExists()
        {
            // Arrange
            var client = new Client { Id = "test-client-id" };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockOrderManager.Setup(och => och.CancelOrderAsync(It.IsAny<int>(), client, It.IsAny<CancellationToken>()));
            // Act
            var result = await orderController.CancelOrder(1, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task ManagerGetOrderById_ValidId_ReturnsOkWithOrder()
        {
            // Arrange
            var order = new OrderResponse { Id = 1, DeliveryAddress = "Address 1" };
            mockOrderManager.Setup(moch => moch.GetOrderByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            // Act
            var result = await orderController.ManagerGetOrderById(1, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(order));
        }
        [Test]
        public async Task ManagerGetOrderById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            mockOrderManager.Setup(moch => moch.GetOrderByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()));
            // Act
            var result = await orderController.ManagerGetOrderById(2, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
        [Test]
        public async Task ManagerGetPaginatedOrders_ReturnsOkWithPaginatedOrders()
        {
            // Arrange
            var paginationRequest = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            var orders = new List<OrderResponse>
    {
        new OrderResponse { Id = 1, DeliveryAddress = "Address 1" }
    };
            mockOrderManager.Setup(moch => moch.GetPaginatedOrdersAsync(It.IsAny<GetOrdersFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(orders);

            // Act
            var result = await orderController.ManagerGetPaginatedOrders(paginationRequest, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(orders));
        }
        [Test]
        public async Task ManagerGetOrderAmount_ReturnsAmount_WhenManagerRequests()
        {
            // Arrange
            var expectedAmount = 10;
            var filter = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            mockOrderManager.Setup(om => om.GetOrderAmountAsync(It.IsAny<GetOrdersFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAmount);
            // Act
            var result = await orderController.ManagerGetOrderAmount(filter, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedAmount));
        }
        [Test]
        public async Task ManagerUpdateOrder_ReturnsOkWithUpdatedOrder()
        {
            // Arrange
            var updateRequest = new ManagerUpdateOrderRequest { Id = 1, DeliveryAddress = "Updated Address" };
            var orderResponse = new OrderResponse { Id = 1, DeliveryAddress = "Updated Address" };
            mockOrderManager.Setup(moch => moch.UpdateOrderAsync(It.IsAny<ManagerUpdateOrderRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(orderResponse);
            // Act
            var result = await orderController.ManagerUpdateOrder(updateRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(orderResponse));
        }
        [Test]
        public async Task ManagerCancelOrder_ReturnsOk_WhenOrderExists()
        {
            // Arrange
            mockOrderManager.Setup(moch => moch.CancelOrderAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()));
            // Act
            var result = await orderController.ManagerCancelOrder(1, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}