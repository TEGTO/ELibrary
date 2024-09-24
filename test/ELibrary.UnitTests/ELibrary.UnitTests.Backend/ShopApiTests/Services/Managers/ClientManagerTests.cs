using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Domain.Dtos.Client;

namespace ShopApi.Services.Facades.Tests
{
    [TestFixture]
    public class ClientManagerTests
    {
        private Mock<IClientService> mockClientService;
        private Mock<IMapper> mockMapper;
        private ClientManager clientManager;

        [SetUp]
        public void SetUp()
        {
            mockClientService = new Mock<IClientService>();
            mockMapper = new Mock<IMapper>();
            clientManager = new ClientManager(mockClientService.Object, mockMapper.Object);
        }

        [Test]
        public async Task GetClientForUserAsync_ValidId_ReturnsClientResponse()
        {
            // Arrange
            var userId = "user-id";
            var client = new Client { UserId = userId };
            var clientResponse = new ClientResponse { Id = userId };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockMapper.Setup(m => m.Map<ClientResponse>(client)).Returns(clientResponse);
            // Act
            var result = await clientManager.GetClientForUserAsync(userId, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(clientResponse));
            mockClientService.Verify(cs => cs.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(m => m.Map<ClientResponse>(client), Times.Once);
        }
        [Test]
        public async Task CreateClientForUserAsync_ValidRequest_ReturnsClientResponse()
        {
            // Arrange
            var userId = "user-id";
            var createRequest = new CreateClientRequest { Name = "Admin Client" };
            var client = new Client { UserId = userId };
            var clientResponse = new ClientResponse { Id = userId };
            mockMapper.Setup(m => m.Map<Client>(createRequest)).Returns(client);
            mockClientService.Setup(cs => cs.CreateClientAsync(client, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockMapper.Setup(m => m.Map<ClientResponse>(client)).Returns(clientResponse);
            // Act
            var result = await clientManager.CreateClientForUserAsync(userId, createRequest, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(clientResponse));
            mockClientService.Verify(cs => cs.CreateClientAsync(client, It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(m => m.Map<ClientResponse>(client), Times.Once);
        }
        [Test]
        public async Task UpdateClientForUserAsync_ValidRequest_ReturnsUpdatedClientResponse()
        {
            // Arrange
            var userId = "user-id";
            var updateRequest = new UpdateClientRequest { Name = "Updated Admin Client" };
            var client = new Client { UserId = userId };
            var updatedClientResponse = new ClientResponse { Id = userId };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockMapper.Setup(m => m.Map<Client>(updateRequest)).Returns(client);
            mockClientService.Setup(cs => cs.UpdateClientAsync(client, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            mockMapper.Setup(m => m.Map<ClientResponse>(client)).Returns(updatedClientResponse);
            // Act
            var result = await clientManager.UpdateClientForUserAsync(userId, updateRequest, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(updatedClientResponse));
            mockClientService.Verify(cs => cs.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            mockClientService.Verify(cs => cs.UpdateClientAsync(client, It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(m => m.Map<ClientResponse>(client), Times.Once);
        }
        [Test]
        public void UpdateClientForUserAsync_ClientNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var userId = "non-existent-client";
            var updateRequest = new UpdateClientRequest { Name = "Non-existent Client" };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client)null);
            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await clientManager.UpdateClientForUserAsync(userId, updateRequest, CancellationToken.None));
            mockClientService.Verify(cs => cs.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task DeleteClientForUserAsync_ValidUserId_DeletesClient()
        {
            // Arrange
            var userId = "test-user-id";
            var client = new Client { UserId = userId };
            mockClientService.Setup(cs => cs.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            // Act
            await clientManager.DeleteClientForUserAsync(userId, CancellationToken.None);
            // Assert
            mockClientService.Verify(cs => cs.GetClientByUserIdAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
            mockClientService.Verify(cs => cs.DeleteClientAsync(client.Id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}