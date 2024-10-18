using AutoMapper;
using Moq;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;
using UserEntities.Domain.Entities;

namespace UserApi.Command.Admin.GetUserByInfo.Tests
{
    [TestFixture]
    internal class GetUserByInfoQueryHandlerTests
    {
        private Mock<IAuthService> authServiceMock;
        private Mock<IMapper> mapperMock;
        private GetUserByInfoQueryHandler getUserByInfoQueryHandler;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            authServiceMock = new Mock<IAuthService>();

            getUserByInfoQueryHandler = new GetUserByInfoQueryHandler(authServiceMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task Handle_UserExists_ReturnsAdminUserResponse()
        {
            // Arrange
            var login = "adminuser";
            var user = new User { UserName = login, Email = "adminuser@example.com" };
            var roles = new List<string> { "Admin" };
            var adminResponse = new AdminUserResponse { Email = "adminuser@example.com" };
            authServiceMock.Setup(a => a.GetUserByUserInfoAsync(login)).ReturnsAsync(user);
            mapperMock.Setup(m => m.Map<AdminUserResponse>(user)).Returns(adminResponse);
            authServiceMock.Setup(a => a.GetUserRolesAsync(user)).ReturnsAsync(roles);
            // Act
            var result = await getUserByInfoQueryHandler.Handle(new GetUserByInfoQuery(login), CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Email, Is.EqualTo("adminuser@example.com"));
            Assert.That(result.Roles, Is.EqualTo(roles));
        }
        [Test]
        public void Handle_UserDoesNotExist_ThrowsKeyNotFoundException()
        {
            // Arrange
            var login = "nonexistentuser";
            authServiceMock.Setup(a => a.GetUserByUserInfoAsync(login)).ReturnsAsync((User)null);
            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await getUserByInfoQueryHandler.Handle(new GetUserByInfoQuery(login), CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("User is not found!"));
        }
    }
}