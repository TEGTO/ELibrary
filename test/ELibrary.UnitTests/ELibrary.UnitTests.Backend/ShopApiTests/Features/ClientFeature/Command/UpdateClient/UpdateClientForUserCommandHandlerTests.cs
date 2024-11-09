using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.ClientFeature.Dtos;
using ShopApi.Features.ClientFeature.Services;

namespace ShopApi.Features.ClientFeature.Command.UpdateClient.Tests
{
    [TestFixture]
    public class UpdateClientForUserCommandHandlerTests
    {
        private Mock<IClientService> mockClientService;
        private Mock<IMapper> mockMapper;
        private UpdateClientForUserCommandHandler handler;

        [SetUp]
        public void SetUp()
        {
            mockClientService = new Mock<IClientService>();
            mockMapper = new Mock<IMapper>();
            handler = new UpdateClient.UpdateClientForUserCommandHandler(mockClientService.Object, mockMapper.Object);
        }

        [Test]
        public async Task Handle_ExistingUserId_UpdatesAndReturnsClientResponse()
        {
            // Arrange
            var command = new UpdateClientForUserCommand("user123", new UpdateClientRequest { Name = "Updated Name" });
            var updatedClient = new Client { Id = "1", UserId = "user123", Name = "Updated Name" };
            var expectedResponse = new ClientResponse { Id = "1", UserId = "user123", Name = "Updated Name" };
            mockMapper.Setup(m => m.Map<Client>(command.Request)).Returns(updatedClient);
            mockClientService.Setup(s => s.UpdateClientAsync(updatedClient, It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedClient);
            mockMapper.Setup(m => m.Map<ClientResponse>(updatedClient)).Returns(expectedResponse);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(expectedResponse));
            mockClientService.Verify(s => s.UpdateClientAsync(updatedClient, It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(m => m.Map<ClientResponse>(updatedClient), Times.Once);
        }
    }
}