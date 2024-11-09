using LibraryShopEntities.Domain.Entities.Shop;
using LibraryShopEntities.Repositories.Shop;
using Moq;

namespace ShopApi.Features.ClientFeature.Services.Tests
{
    [TestFixture]
    internal class ClientServiceTests
    {
        private Mock<IClientRepository> repositoryMock;
        private ClientService service;
        private CancellationToken cancellationToken;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IClientRepository>();
            service = new ClientService(repositoryMock.Object);
            cancellationToken = new CancellationToken();
        }

        [Test]
        public async Task GetClientByUserIdAsync_ValidUserId_ReturnsClient()
        {
            // Arrange
            var userId = "user123";
            var client = new Client { Id = "client1", UserId = userId };
            repositoryMock.Setup(repo => repo.GetClientByUserIdAsync(userId, cancellationToken)).ReturnsAsync(client);

            // Act
            var result = await service.GetClientByUserIdAsync(userId, cancellationToken);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result!.Id, Is.EqualTo(client.Id));
            repositoryMock.Verify(repo => repo.GetClientByUserIdAsync(userId, cancellationToken), Times.Once);
        }
        [Test]
        public async Task GetClientByUserIdAsync_InvalidUserId_ReturnsNull()
        {
            // Arrange
            var userId = "nonexistent_user";
            repositoryMock.Setup(repo => repo.GetClientByUserIdAsync(userId, cancellationToken)).ReturnsAsync((Client?)null);
            // Act
            var result = await service.GetClientByUserIdAsync(userId, cancellationToken);
            // Assert
            Assert.IsNull(result);
            repositoryMock.Verify(repo => repo.GetClientByUserIdAsync(userId, cancellationToken), Times.Once);
        }
        [Test]
        public async Task CreateClientAsync_ValidClient_CreatesClient()
        {
            // Arrange
            var client = new Client { Id = "client1", UserId = "user123" };
            repositoryMock.Setup(repo => repo.CreateClientAsync(client, cancellationToken)).ReturnsAsync(client);
            // Act
            var result = await service.CreateClientAsync(client, cancellationToken);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(client.Id));
            repositoryMock.Verify(repo => repo.CreateClientAsync(client, cancellationToken), Times.Once);
        }
        [Test]
        public async Task UpdateClientAsync_ExistingClient_UpdatesClient()
        {
            // Arrange
            var client = new Client { Id = "client1", UserId = "user123", Name = "Updated Name" };
            var clientInDb = new Client { Id = "client1", UserId = "user123", Name = "Original Name" };
            repositoryMock.Setup(repo => repo.GetClientByUserIdAsync(client.UserId, cancellationToken)).ReturnsAsync(clientInDb);
            repositoryMock.Setup(repo => repo.UpdateClientAsync(clientInDb, cancellationToken)).ReturnsAsync(clientInDb);
            // Act
            var result = await service.UpdateClientAsync(client, cancellationToken);
            // Assert
            Assert.That(clientInDb.Name, Is.EqualTo(client.Name));
            repositoryMock.Verify(repo => repo.GetClientByUserIdAsync(client.UserId, cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.UpdateClientAsync(clientInDb, cancellationToken), Times.Once);
        }
        [Test]
        public void UpdateClientAsync_NonexistentClient_ThrowsInvalidOperationException()
        {
            // Arrange
            var client = new Client { Id = "nonexistent_client", UserId = "user123" };
            repositoryMock.Setup(repo => repo.GetClientByUserIdAsync(client.UserId, cancellationToken)).ReturnsAsync((Client?)null);
            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.UpdateClientAsync(client, cancellationToken));
            repositoryMock.Verify(repo => repo.GetClientByUserIdAsync(client.UserId, cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.UpdateClientAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        [Test]
        public async Task DeleteClientAsync_ExistingClient_DeletesClient()
        {
            // Arrange
            var client = new Client { Id = "client1", UserId = "user123" };
            repositoryMock.Setup(repo => repo.GetClientByUserIdAsync(client.Id, cancellationToken)).ReturnsAsync(client);
            // Act
            await service.DeleteClientAsync(client.Id, cancellationToken);
            // Assert
            repositoryMock.Verify(repo => repo.GetClientByUserIdAsync(client.Id, cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.DeleteClientAsync(client, cancellationToken), Times.Once);
        }
        [Test]
        public async Task DeleteClientAsync_NonexistentClient_DoesNothing()
        {
            // Arrange
            var clientId = "nonexistent_client";
            repositoryMock.Setup(repo => repo.GetClientByUserIdAsync(clientId, cancellationToken)).ReturnsAsync((Client?)null);
            // Act
            await service.DeleteClientAsync(clientId, cancellationToken);
            // Assert
            repositoryMock.Verify(repo => repo.GetClientByUserIdAsync(clientId, cancellationToken), Times.Once);
            repositoryMock.Verify(repo => repo.DeleteClientAsync(It.IsAny<Client>(), cancellationToken), Times.Never);
        }
    }
}