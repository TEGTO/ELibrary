using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.Services;
using ShopApi.Features.ClientFeature.Command.CreateClient;
using ShopApi.Features.ClientFeature.Command.GetClient;
using ShopApi.Features.ClientFeature.Command.UpdateClient;
using ShopApi.Features.ClientFeature.Dtos;
using System.Security.Claims;

namespace ShopApi.Controllers.Tests
{
    [TestFixture]
    internal class ClientControllerTests
    {
        private Mock<IMediator> mockMediator;
        private Mock<ICacheService> mockCacheService;
        private ClientController clientController;

        [SetUp]
        public void SetUp()
        {
            mockMediator = new Mock<IMediator>();
            mockCacheService = new Mock<ICacheService>();

            clientController = new ClientController(mockMediator.Object, mockCacheService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
            }, "mock"));

            clientController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Test]
        public async Task GetClient_ReturnsOkWithClientResponse()
        {
            // Arrange
            var clientResponse = new GetClientResponse { Client = new ClientResponse { Id = "test-client-id" } };
            mockMediator
                .Setup(m => m.Send(It.IsAny<GetClientForUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientResponse);
            // Act
            var result = await clientController.GetClient(CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(clientResponse));
        }
        [Test]
        public async Task CreateClient_ReturnsCreatedWithClientResponse()
        {
            // Arrange
            var createRequest = new CreateClientRequest { Name = "John" };
            var clientResponse = new ClientResponse { Id = "test-client-id" };
            mockMediator
                .Setup(m => m.Send(It.IsAny<CreateClientForUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientResponse);
            // Act
            var result = await clientController.CreateClient(createRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<CreatedResult>(result.Result);
            var createdResult = result.Result as CreatedResult;
            Assert.That(createdResult?.Value, Is.EqualTo(clientResponse));
        }
        [Test]
        public async Task UpdateClient_ReturnsOkWithUpdatedClientResponse()
        {
            // Arrange
            var updateRequest = new UpdateClientRequest { Name = "Updated Name" };
            var updatedClientResponse = new ClientResponse { Id = "test-client-id" };
            mockMediator
                .Setup(m => m.Send(It.IsAny<UpdateClientForUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedClientResponse);
            // Act
            var result = await clientController.UpdateClient(updateRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(updatedClientResponse));
        }
        [Test]
        public async Task AdminGetClient_ReturnsOkWithClientResponse()
        {
            // Arrange
            var clientResponse = new GetClientResponse { Client = new ClientResponse { Id = "admin-client-id" } };
            mockMediator
                .Setup(m => m.Send(It.IsAny<GetClientForUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientResponse);
            // Act
            var result = await clientController.AdminGetClient("admin-user-id", CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(clientResponse));
        }
        [Test]
        public async Task AdminCreateClient_ReturnsCreatedWithClientResponse()
        {
            // Arrange
            var createRequest = new CreateClientRequest { Name = "Admin Client" };
            var clientResponse = new ClientResponse { Id = "admin-client-id" };
            mockMediator
                .Setup(m => m.Send(It.IsAny<CreateClientForUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientResponse);
            // Act
            var result = await clientController.AdminCreateClient("admin-user-id", createRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<CreatedResult>(result.Result);
            var createdResult = result.Result as CreatedResult;
            Assert.That(createdResult?.Value, Is.EqualTo(clientResponse));
        }
        [Test]
        public async Task AdminUpdateClient_ReturnsOkWithUpdatedClientResponse()
        {
            // Arrange
            var updateRequest = new UpdateClientRequest { Name = "Updated Admin Client" };
            var updatedClientResponse = new ClientResponse { Id = "admin-client-id" };
            mockMediator
                .Setup(m => m.Send(It.IsAny<UpdateClientForUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedClientResponse);
            // Act
            var result = await clientController.AdminUpdateClient("admin-user-id", updateRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(updatedClientResponse));
        }
    }
}