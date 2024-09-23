using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopApi.Domain.Dtos.Client;
using ShopApi.Services;
using System.Security.Claims;

namespace ShopApi.Controllers.Tests
{
    [TestFixture]
    internal class ClientControllerTests
    {
        private Mock<IMapper> mockMapper;
        private Mock<IClientService> mockClientService;
        private ClientController clientController;

        [SetUp]
        public void SetUp()
        {
            mockMapper = new Mock<IMapper>();
            mockClientService = new Mock<IClientService>();

            clientController = new ClientController(mockMapper.Object, mockClientService.Object);

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
            var client = new Client { UserId = "test-user-id" };
            var clientResponse = new ClientResponse { Id = "test-client-id" };

            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockMapper.Setup(m => m.Map<ClientResponse>(It.IsAny<Client>())).Returns(clientResponse);
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
            Client? client = null;
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            // Act
            var result = await clientController.GetClient(CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
        [Test]
        public async Task CreateClient_ReturnsCreatedWithClientResponse()
        {
            // Arrange
            var createRequest = new CreateClientRequest { Name = "John" };
            var client = new Client { UserId = "test-user-id" };
            var clientResponse = new ClientResponse { Id = "test-client-id" };
            mockMapper.Setup(m => m.Map<Client>(createRequest)).Returns(client);
            mockClientService.Setup(cs => cs.CreateClientAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockMapper.Setup(m => m.Map<ClientResponse>(client)).Returns(clientResponse);
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
            var existingClient = new Client { UserId = "test-user-id" };
            var updatedClientResponse = new ClientResponse { Id = "test-client-id" };
            var client = new Client { UserId = "test-user-id" };
            mockMapper.Setup(m => m.Map<Client>(updateRequest)).Returns(client);
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingClient);
            mockClientService.Setup(cs => cs.UpdateClientAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingClient);
            mockMapper.Setup(m => m.Map<ClientResponse>(existingClient)).Returns(updatedClientResponse);
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
            var client = new Client { Id = "test-client-id", UserId = "test-user-id" };

            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockClientService.Setup(cs => cs.DeleteClientAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
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
            var client = new Client { UserId = "admin-user-id" };
            var clientResponse = new ClientResponse { Id = "admin-client-id" };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockMapper.Setup(m => m.Map<ClientResponse>(client)).Returns(clientResponse);
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
            var client = new Client { UserId = "admin-user-id" };
            var clientResponse = new ClientResponse { Id = "admin-client-id" };
            mockMapper.Setup(m => m.Map<Client>(createRequest)).Returns(client);
            mockClientService.Setup(cs => cs.CreateClientAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockMapper.Setup(m => m.Map<ClientResponse>(client)).Returns(clientResponse);
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
            var existingClient = new Client { UserId = "admin-user-id" };
            var updatedClientResponse = new ClientResponse { Id = "admin-client-id" };

            mockMapper.Setup(m => m.Map<Client>(updateRequest)).Returns(existingClient);
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingClient);
            mockClientService.Setup(cs => cs.UpdateClientAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingClient);
            mockMapper.Setup(m => m.Map<ClientResponse>(existingClient)).Returns(updatedClientResponse);
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
            var client = new Client { Id = "admin-client-id", UserId = "admin-user-id" };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockClientService.Setup(cs => cs.DeleteClientAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            // Act
            var result = await clientController.AdminDeleteClient("admin-user-id", CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}