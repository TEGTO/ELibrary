using DatabaseControl.Repositories;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using MockQueryable.Moq;
using Moq;

namespace LibraryShopEntities.Repositories.Shop.Tests
{
    [TestFixture]
    internal class ClientRepositoryTests
    {
        private Mock<IDatabaseRepository<ShopDbContext>> mockRepository;
        private ClientRepository clientRepository;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new Mock<IDatabaseRepository<ShopDbContext>>();

            clientRepository = new ClientRepository(mockRepository.Object);
        }

        private static IQueryable<T> GetDbSetMock<T>(List<T> data) where T : class
        {
            return data.AsQueryable().BuildMock();
        }

        [Test]
        [TestCase("test-user-id", true, Description = "Returns a client when the client exists.")]
        [TestCase("non-existing-user-id", false, Description = "Returns null when the client does not exist.")]
        public async Task GetClientByUserIdAsync_TestCases(string userId, bool shouldExist)
        {
            // Arrange
            var clients = shouldExist
                ? new List<Client> { new Client { Id = "client-id", UserId = userId } }
                : new List<Client>();

            var mockQueryable = GetDbSetMock(clients);
            mockRepository.Setup(repo => repo.GetQueryableAsync<Client>(It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockQueryable);

            // Act
            var result = await clientRepository.GetClientByUserIdAsync(userId, CancellationToken.None);

            // Assert
            if (shouldExist)
            {
                Assert.IsNotNull(result);
                Assert.That(result!.UserId, Is.EqualTo(userId));
            }
            else
            {
                Assert.IsNull(result);
            }
        }

        [Test]
        [TestCase("client-id", "test-user-id", Description = "Creates a new client and returns it.")]
        public async Task CreateClientAsync_TestCases(string clientId, string userId)
        {
            // Arrange
            var client = new Client { Id = clientId, UserId = userId };
            mockRepository.Setup(repo => repo.AddAsync(client, It.IsAny<CancellationToken>()))
                .ReturnsAsync(client);

            // Act
            var result = await clientRepository.CreateClientAsync(client, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(clientId));
            Assert.That(result.UserId, Is.EqualTo(userId));
        }

        [Test]
        [TestCase("client-id", "test-user-id", "Updated Name", Description = "Updates an existing client and returns it.")]
        public async Task UpdateClientAsync_TestCases(string clientId, string userId, string updatedName)
        {
            // Arrange
            var client = new Client { Id = clientId, UserId = userId };
            var updatedClient = new Client { Id = clientId, Name = updatedName };
            mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedClient);

            // Act
            var result = await clientRepository.UpdateClientAsync(client, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(clientId));
            Assert.That(result.Name, Is.EqualTo(updatedName));
        }

        [Test]
        [TestCase("client-id", "test-user-id", Description = "Deletes an existing client.")]
        public async Task DeleteClientAsync_TestCases(string clientId, string userId)
        {
            // Arrange
            var client = new Client { Id = clientId, UserId = userId };

            // Act
            await clientRepository.DeleteClientAsync(client, CancellationToken.None);

            // Assert
            mockRepository.Verify(repo => repo.DeleteAsync(client, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}