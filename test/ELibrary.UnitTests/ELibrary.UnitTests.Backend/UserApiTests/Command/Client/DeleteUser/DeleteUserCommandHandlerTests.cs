using Moq;
using System.Security.Claims;
using UserApi.Services;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Client.DeleteUser.Tests
{
    [TestFixture]
    internal class DeleteUserCommandHandlerTests
    {
        private Mock<IUserService> userServiceMock;
        private DeleteUserCommandHandler deleteUserCommandHandler;

        [SetUp]
        public void SetUp()
        {
            userServiceMock = new Mock<IUserService>();

            deleteUserCommandHandler = new DeleteUserCommandHandler(userServiceMock.Object);
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
            userServiceMock.Setup(s => s.GetUserAsync(claimsPrincipal, CancellationToken.None))
                           .ReturnsAsync(user);
            // Act
            await deleteUserCommandHandler.Handle(new DeleteUserCommand(claimsPrincipal), CancellationToken.None);
            // Assert
            userServiceMock.Verify(s => s.GetUserAsync(claimsPrincipal, CancellationToken.None), Times.Once);
            userServiceMock.Verify(s => s.DeleteUserAsync(user, CancellationToken.None), Times.Once);
        }
    }
}