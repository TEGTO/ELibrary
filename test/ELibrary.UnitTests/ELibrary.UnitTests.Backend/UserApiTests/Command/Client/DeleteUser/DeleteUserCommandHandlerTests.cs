using Moq;
using System.Security.Claims;
using UserApi.Services;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Client.DeleteUser.Tests
{
    [TestFixture]
    internal class DeleteUserCommandHandlerTests
    {
        private Mock<IAuthService> authServiceMock;
        private DeleteUserCommandHandler deleteUserCommandHandler;

        [SetUp]
        public void SetUp()
        {
            authServiceMock = new Mock<IAuthService>();

            deleteUserCommandHandler = new DeleteUserCommandHandler(authServiceMock.Object);
        }

        [Test]
        public async Task Handle_UserExists_DeletesUser()
        {
            // Arrange
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "12345")
            }));
            var user = new User { Id = "12345", Email = "testuser@example.com" };
            authServiceMock.Setup(s => s.GetUserAsync(claimsPrincipal))
                           .ReturnsAsync(user);
            // Act
            await deleteUserCommandHandler.Handle(new DeleteUserCommand(claimsPrincipal), CancellationToken.None);
            // Assert
            authServiceMock.Verify(s => s.GetUserAsync(claimsPrincipal), Times.Once);
            authServiceMock.Verify(s => s.DeleteUserAsync(user), Times.Once);
        }
    }
}