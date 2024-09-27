using LibraryShopEntities.Domain.Dtos.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopApi.Features.ClientFeature.Dtos;
using ShopApi.Features.ClientFeature.Services;
using System.Security.Claims;

namespace ShopApi.Controllers.Tests
{
    [TestFixture]
    internal class ClientControllerTests
    {
        private Mock<IClientManager> mockClientManager;
        private ClientController clientController;

        [SetUp]
        public void SetUp()
        {
            mockClientManager = new Mock<IClientManager>();

            clientController = new ClientController(mockClientManager.Object);

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
            var clientResponse = new ClientResponse { Id = "test-client-id" };
            mockClientManager.Setup(cf => cf.GetClientForUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientResponse);
            // Act
            var result = await clientController.GetClient(CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(clientResponse));
        }
        [Test]
        public async Task GetClient_ReturnsNotFoundResponse()
        {
            // Arrange
            mockClientManager.Setup(cf => cf.GetClientForUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ClientResponse)null);
            // Act
            var result = await clientController.GetClient(CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }
        [Test]
        public async Task CreateClient_ReturnsCreatedWithClientResponse()
        {
            // Arrange
            var createRequest = new CreateClientRequest { Name = "John" };
            var clientResponse = new ClientResponse { Id = "test-client-id" };
            mockClientManager.Setup(cf => cf.CreateClientForUserAsync(It.IsAny<string>(), createRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientResponse);
            // Act
            var result = await clientController.CreateClient(createRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<CreatedResult>(result.Result);
            var createdResult = result.Result as CreatedResult;
            Assert.That(createdResult.Value, Is.EqualTo(clientResponse));
        }
        [Test]
        public async Task UpdateClient_ReturnsOkWithUpdatedClientResponse()
        {
            // Arrange
            var updateRequest = new UpdateClientRequest { Name = "Updated Name" };
            var updatedClientResponse = new ClientResponse { Id = "test-client-id" };
            mockClientManager.Setup(cf => cf.UpdateClientForUserAsync(It.IsAny<string>(), updateRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedClientResponse);
            // Act
            var result = await clientController.UpdateClient(updateRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(updatedClientResponse));
        }
        [Test]
        public async Task DeleteClient_ReturnsOk()
        {
            // Arrange
            mockClientManager.Setup(cf => cf.DeleteClientForUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            // Act
            var result = await clientController.DeleteClient(CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task AdminGetClient_ReturnsOkWithClientResponse()
        {
            // Arrange
            var clientResponse = new ClientResponse { Id = "admin-client-id" };
            mockClientManager.Setup(acf => acf.GetClientForUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientResponse);
            // Act
            var result = await clientController.AdminGetClient("admin-user-id", CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(clientResponse));
        }
        [Test]
        public async Task AdminCreateClient_ReturnsCreatedWithClientResponse()
        {
            // Arrange
            var createRequest = new CreateClientRequest { Name = "Admin Client" };
            var clientResponse = new ClientResponse { Id = "admin-client-id" };
            mockClientManager.Setup(acf => acf.CreateClientForUserAsync(It.IsAny<string>(), createRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientResponse);
            // Act
            var result = await clientController.AdminCreateClient("admin-user-id", createRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<CreatedResult>(result.Result);
            var createdResult = result.Result as CreatedResult;
            Assert.That(createdResult.Value, Is.EqualTo(clientResponse));
        }
        [Test]
        public async Task AdminUpdateClient_ReturnsOkWithUpdatedClientResponse()
        {
            // Arrange
            var updateRequest = new UpdateClientRequest { Name = "Updated Admin Client" };
            var updatedClientResponse = new ClientResponse { Id = "admin-client-id" };
            mockClientManager.Setup(acf => acf.UpdateClientForUserAsync(It.IsAny<string>(), updateRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedClientResponse);
            // Act
            var result = await clientController.AdminUpdateClient("admin-user-id", updateRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(updatedClientResponse));
        }
        [Test]
        public async Task AdminDeleteClient_ReturnsOk()
        {
            // Arrange
            mockClientManager.Setup(acf => acf.DeleteClientForUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            // Act
            var result = await clientController.AdminDeleteClient("admin-user-id", CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}