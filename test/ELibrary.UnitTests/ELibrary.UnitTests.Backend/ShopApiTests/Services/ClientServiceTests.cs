using LibraryShopEntities.Domain.Entities.Shop;
using MockQueryable.Moq;
using Moq;
using ShopApi.Repositories;

namespace ShopApi.Services.Tests
{
    [TestFixture]
    internal class ClientServiceTests
    {
        private Mock<IShopDatabaseRepository> mockRepository;
        private ClientService clientService;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new Mock<IShopDatabaseRepository>();
            clientService = new ClientService(mockRepository.Object);
        }
        private static IQueryable<T> GetDbSetMock<T>(List<T> data) where T : class
        {
            return data.AsQueryable().BuildMock();
        }

        [Test]
        public async Task GetClientByUserIdAsync_ReturnsClient_WhenClientExists()
        {
            // Arrange
            var userId = "test-user-id";
            var client = new Client { Id = "client-id", UserId = userId };
            var mockQueryable = GetDbSetMock(new List<Client> { client });
            mockRepository.Setup(repo => repo.GetQueryableAsync<Client>(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockQueryable);
            // Act
            var result = await clientService.GetClientByUserIdAsync(userId, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result?.UserId, Is.EqualTo(userId));
        }
        [Test]
        public async Task GetClientByUserIdAsync_ReturnsNull_WhenClientDoesNotExist()
        {
            // Arrange
            var userId = "non-existing-user-id";
            var mockQueryable = GetDbSetMock(new List<Client>());
            mockRepository.Setup(repo => repo.GetQueryableAsync<Client>(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockQueryable);
            // Act
            var result = await clientService.GetClientByUserIdAsync(userId, CancellationToken.None);
            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task CreateClientAsync_ReturnsCreatedClient()
        {
            // Arrange
            var client = new Client { Id = "client-id", UserId = "test-user-id" };
            mockRepository.Setup(repo => repo.AddAsync(client, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);
            // Act
            var result = await clientService.CreateClientAsync(client, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(client.Id));
        }
        [Test]
        public async Task UpdateClientAsync_ReturnsUpdatedClient()
        {
            // Arrange
            var client = new Client { Id = "client-id", UserId = "test-user-id" };
            var updatedClient = new Client { Id = "client-id", Name = "Updated Name" };
            var mockQueryable = GetDbSetMock(new List<Client> { client });
            mockRepository.Setup(repo => repo.GetQueryableAsync<Client>(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockQueryable);
            mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedClient);
            // Act
            var result = await clientService.UpdateClientAsync(updatedClient, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo(updatedClient.Name));
        }
        [Test]
        public async Task DeleteClientAsync_DeletesClient()
        {
            // Arrange
            var client = new Client { Id = "client-id", UserId = "test-user-id" };
            var mockQueryable = GetDbSetMock(new List<Client> { client });
            mockRepository.Setup(repo => repo.GetQueryableAsync<Client>(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockQueryable);
            // Act
            await clientService.DeleteClientAsync(client.Id, CancellationToken.None);
            // Assert
            mockRepository.Verify(repo => repo.DeleteAsync(client, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}