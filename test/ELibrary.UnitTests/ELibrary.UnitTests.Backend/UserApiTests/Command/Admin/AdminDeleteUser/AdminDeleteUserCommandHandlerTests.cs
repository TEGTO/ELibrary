using Microsoft.AspNetCore.Identity;
using Moq;
using Shared.Exceptions;
using UserApi.Services;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Admin.AdminDeleteUser.Tests
{
    [TestFixture]
    internal class AdminDeleteUserCommandHandlerTests
    {
        private Mock<IAuthService> authServiceMock;
        private AdminDeleteUserCommandHandler adminDeleteUserCommandHandler;

        [SetUp]
        public void SetUp()
        {
            authServiceMock = new Mock<IAuthService>();

            adminDeleteUserCommandHandler = new AdminDeleteUserCommandHandler(authServiceMock.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_DeletesUserSuccessfully()
        {
            // Arrange
            var user = new User { UserName = "user1" };
            authServiceMock.Setup(a => a.GetUserByUserInfoAsync(It.IsAny<string>())).ReturnsAsync(user);
            authServiceMock.Setup(a => a.DeleteUserAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
            // Act
            await adminDeleteUserCommandHandler.Handle(new AdminDeleteUserCommand("user1"), CancellationToken.None);
            // Assert
            authServiceMock.Verify(a => a.DeleteUserAsync(user), Times.Once);
        }
        [Test]
        public void Handle_FailedDelete_ThrowsAuthorizationException()
        {
            // Arrange
            var user = new User { UserName = "user1" };
            var identityErrors = new List<IdentityError> { new IdentityError { Description = "Deletion failed" } };
            authServiceMock.Setup(a => a.GetUserByUserInfoAsync(It.IsAny<string>())).ReturnsAsync(user);
            authServiceMock.Setup(a => a.DeleteUserAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));
            // Act & Assert
            var ex = Assert.ThrowsAsync<AuthorizationException>(async () => await adminDeleteUserCommandHandler.Handle(new AdminDeleteUserCommand("user1"), CancellationToken.None));
            Assert.That(ex.Errors.First(), Is.EqualTo("Deletion failed"));
        }
    }
}