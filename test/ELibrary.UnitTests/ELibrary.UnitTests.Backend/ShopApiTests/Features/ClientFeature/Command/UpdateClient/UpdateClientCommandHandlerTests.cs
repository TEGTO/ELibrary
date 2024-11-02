using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.ClientFeature.Dtos;
using ShopApi.Features.ClientFeature.Services;

namespace ShopApi.Features.ClientFeature.Command.UpdateClient.Tests
{
    [TestFixture]
    public class UpdateClientCommandHandlerTests
    {
        private Mock<IClientService> mockClientService;
        private Mock<IMapper> mockMapper;
        private UpdateClientCommandHandler handler;

        [SetUp]
        public void SetUp()
        {
            mockClientService = new Mock<IClientService>();
            mockMapper = new Mock<IMapper>();
            handler = new UpdateClientCommandHandler(mockClientService.Object, mockMapper.Object);
        }

        [Test]
        public async Task Handle_ExistingUserId_UpdatesAndReturnsClientResponse()
        {
            // Arrange
            var command = new UpdateClientCommand("user123", new UpdateClientRequest { Name = "Updated Name" });
            var existingClient = new Client { Id = "1", UserId = "user123", Name = "Old Name" };
            var updatedClient = new Client { Id = "1", UserId = "user123", Name = "Updated Name" };
            var expectedResponse = new ClientResponse { Id = "1", UserId = "user123", Name = "Updated Name" };
            mockClientService.Setup(s => s.GetClientByUserIdAsync(command.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingClient);
            mockMapper.Setup(m => m.Map<Client>(command.Request)).Returns(new Client { Name = "Updated Name" });
            mockClientService.Setup(s => s.UpdateClientAsync(existingClient, It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedClient);
            mockMapper.Setup(m => m.Map<ClientResponse>(updatedClient)).Returns(expectedResponse);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(expectedResponse));
            mockClientService.Verify(s => s.GetClientByUserIdAsync(command.UserId, It.IsAny<CancellationToken>()), Times.Once);
            mockClientService.Verify(s => s.UpdateClientAsync(existingClient, It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(m => m.Map<ClientResponse>(updatedClient), Times.Once);
        }
        [Test]
        public void Handle_NonExistingUserId_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = new UpdateClientCommand("nonexistentUser", new UpdateClientRequest { Name = "New Name" });
            mockClientService.Setup(s => s.GetClientByUserIdAsync(command.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Client)null);
            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("Client doesn't exist!"));
            mockClientService.Verify(s => s.GetClientByUserIdAsync(command.UserId, It.IsAny<CancellationToken>()), Times.Once);
            mockClientService.Verify(s => s.UpdateClientAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}