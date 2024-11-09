using Caching.Services;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using LibraryShopEntities.Filters;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopApi.Features.OrderFeature.Command.CreateOrder;
using ShopApi.Features.OrderFeature.Command.GetOrderAmount;
using ShopApi.Features.OrderFeature.Command.GetOrders;
using ShopApi.Features.OrderFeature.Command.ManagerGetOrderAmount;
using ShopApi.Features.OrderFeature.Command.ManagerGetOrderById;
using ShopApi.Features.OrderFeature.Command.ManagerGetPaginatedOrders;
using ShopApi.Features.OrderFeature.Command.ManagerUpdateOrder;
using ShopApi.Features.OrderFeature.Command.UpdateOrder;
using ShopApi.Features.OrderFeature.Dtos;
using System.Security.Claims;

namespace ShopApi.Controllers.Tests
{
    [TestFixture]
    internal class OrderControllerTests
    {
        private Mock<IMediator> mockMediator;
        private Mock<ICacheService> mockCacheService;
        private OrderController orderController;

        [SetUp]
        public void SetUp()
        {
            mockMediator = new Mock<IMediator>();
            mockCacheService = new Mock<ICacheService>();

            mockCacheService.Setup(x => x.Get<object>(It.IsAny<string>())).Returns(null);

            orderController = new OrderController(
                mockMediator.Object,
                mockCacheService.Object
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
            var filter = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            var orders = new List<OrderResponse>
            {
                new OrderResponse { Id = 1, DeliveryAddress = "Address 1" }
            };
            mockMediator.Setup(och => och.Send(It.IsAny<GetOrdersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(orders);
            // Act
            var result = await orderController.GetOrders(filter, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(orders));
        }
        [Test]
        public async Task GetOrderAmount_ReturnsAmount_WhenClientExists()
        {
            // Arrange
            var filter = new GetOrdersFilter { PageNumber = 1, PageSize = 10 };
            var expectedAmount = 5;
            mockMediator.Setup(och => och.Send(It.IsAny<GetOrderAmountQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(expectedAmount);
            // Act
            var result = await orderController.GetOrderAmount(filter, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedAmount));
        }
        [Test]
        public async Task CreateOrder_ReturnsCreated_WhenClientExists()
        {
            // Arrange
            var createRequest = new CreateOrderRequest { DeliveryAddress = "Test Address" };
            var orderResponse = new OrderResponse { Id = 1 };
            mockMediator.Setup(och => och.Send(It.IsAny<CreateOrderCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(orderResponse);
            // Act
            var result = await orderController.CreateOrder(createRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<CreatedResult>(result.Result);
            var createdResult = result.Result as CreatedResult;
            Assert.That(createdResult?.Value, Is.EqualTo(orderResponse));
        }
        [Test]
        public async Task UpdateOrder_ReturnsOk_WhenOrderExists()
        {
            // Arrange
            var client = new Client { Id = "test-client-id" };
            var updateRequest = new ClientUpdateOrderRequest { Id = 1, DeliveryAddress = "Updated Address" };
            var orderResponse = new OrderResponse { Id = 1, DeliveryAddress = "Updated Address" };
            mockMediator.Setup(och => och.Send(It.IsAny<UpdateOrderCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(orderResponse);
            // Act
            var result = await orderController.UpdateOrder(updateRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(orderResponse));
        }
        [Test]
        public async Task CancelOrder_ReturnsOk_WhenOrderExists()
        {
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
            mockMediator.Setup(och => och.Send(It.IsAny<ManagerGetOrderByIdQuery>(), It.IsAny<CancellationToken>()))
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
            mockMediator.Setup(och => och.Send(It.IsAny<ManagerGetPaginatedOrdersQuery>(), It.IsAny<CancellationToken>()))
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
            mockMediator.Setup(och => och.Send(It.IsAny<ManagerGetOrderAmountQuery>(), It.IsAny<CancellationToken>()))
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
            mockMediator.Setup(och => och.Send(It.IsAny<ManagerUpdateOrderCommand>(), It.IsAny<CancellationToken>()))
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
            // Act
            var result = await orderController.ManagerCancelOrder(1, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}