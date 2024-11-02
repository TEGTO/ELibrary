using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.ClientFeature.Dtos;
using ShopApi.Features.ClientFeature.Services;

namespace ShopApi.Features.ClientFeature.Command.CreateClient.Tests
{
    [TestFixture]
    internal class CreateClientCommandHandlerTests
    {
        private Mock<IClientService> mockClientService;
        private Mock<IMapper> mockMapper;
        private CreateClientCommandHandler handler;

        [SetUp]
        public void SetUp()
        {
            mockClientService = new Mock<IClientService>();
            mockMapper = new Mock<IMapper>();
            handler = new CreateClientCommandHandler(mockClientService.Object, mockMapper.Object);
        }

        [Test]
        public async Task Handle_ValidCommand_CreatesClientAndReturnsClientResponse()
        {
            // Arrange
            var command = new CreateClientCommand(
              "user123",
              new CreateClientRequest { Name = "Test Client" }
            );
            var client = new Client { Id = "1", Name = "Test Client", UserId = "user123" };
            var createdClient = new Client { Id = "1", Name = "Test Client", UserId = "user123" };
            var expectedResponse = new ClientResponse { Id = "1", Name = "Test Client" };
            mockMapper.Setup(m => m.Map<Client>(command.Request)).Returns(client);
            mockClientService.Setup(s => s.CreateClientAsync(client, It.IsAny<CancellationToken>())).ReturnsAsync(createdClient);
            mockMapper.Setup(m => m.Map<ClientResponse>(createdClient)).Returns(expectedResponse);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(expectedResponse));
            mockMapper.Verify(m => m.Map<Client>(command.Request), Times.Once);
            mockClientService.Verify(s => s.CreateClientAsync(client, It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(m => m.Map<ClientResponse>(createdClient), Times.Once);
        }
        [Test]
        public async Task Handle_ValidCommand_SetsUserIdOnClient()
        {
            // Arrange
            var command = new CreateClientCommand(
                "user123",
                new CreateClientRequest { Name = "Test Client" }
           );
            var client = new Client { Id = "1", Name = "Test Client" };
            var createdClient = new Client { Id = "1", Name = "Test Client", UserId = "user123" };
            var expectedResponse = new ClientResponse { Id = "1", Name = "Test Client" };
            mockMapper.Setup(m => m.Map<Client>(command.Request)).Returns(client);
            mockClientService.Setup(s => s.CreateClientAsync(client, It.IsAny<CancellationToken>())).ReturnsAsync(createdClient);
            mockMapper.Setup(m => m.Map<ClientResponse>(createdClient)).Returns(expectedResponse);
            // Act
            await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.That(client.UserId, Is.EqualTo(command.UserId));
        }
        [Test]
        public void Handle_NullCommandRequest_ThrowsNullReferenceException()
        {
            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await handler.Handle(null, CancellationToken.None));
        }
    }
}