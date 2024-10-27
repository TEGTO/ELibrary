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
        private Mock<IUserService> userServiceMock;
        private AdminDeleteUserCommandHandler adminDeleteUserCommandHandler;

        [SetUp]
        public void SetUp()
        {
            userServiceMock = new Mock<IUserService>();

            adminDeleteUserCommandHandler = new AdminDeleteUserCommandHandler(userServiceMock.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_DeletesUserSuccessfully()
        {
            // Arrange
            var user = new User { UserName = "user1" };
            userServiceMock.Setup(a => a.GetUserByUserInfoAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
            userServiceMock.Setup(a => a.DeleteUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Success);
            // Act
            await adminDeleteUserCommandHandler.Handle(new AdminDeleteUserCommand("user1"), CancellationToken.None);
            // Assert
            userServiceMock.Verify(a => a.DeleteUserAsync(user, CancellationToken.None), Times.Once);
        }
        [Test]
        public void Handle_FailedDelete_ThrowsAuthorizationException()
        {
            // Arrange
            var user = new User { UserName = "user1" };
            var identityErrors = new List<IdentityError> { new IdentityError { Description = "Deletion failed" } };
            userServiceMock.Setup(a => a.GetUserByUserInfoAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
            userServiceMock.Setup(a => a.DeleteUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));
            // Act & Assert
            var ex = Assert.ThrowsAsync<AuthorizationException>(async () => await adminDeleteUserCommandHandler.Handle(new AdminDeleteUserCommand("user1"), CancellationToken.None));
            Assert.That(ex.Errors.First(), Is.EqualTo("Deletion failed"));
        }
    }
}