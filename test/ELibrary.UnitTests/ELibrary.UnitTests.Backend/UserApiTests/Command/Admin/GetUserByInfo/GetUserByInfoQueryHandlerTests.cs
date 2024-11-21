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
        private Mock<IUserService> userServiceMock;
        private Mock<IUserAuthenticationMethodService> authMethodService;
        private Mock<IMapper> mapperMock;
        private GetUserByInfoQueryHandler getUserByInfoQueryHandler;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            userServiceMock = new Mock<IUserService>();
            authMethodService = new Mock<IUserAuthenticationMethodService>();

            getUserByInfoQueryHandler = new GetUserByInfoQueryHandler(userServiceMock.Object, authMethodService.Object, mapperMock.Object);
        }

        [Test]
        public async Task Handle_UserExists_ReturnsAdminUserResponse()
        {
            // Arrange
            var login = "adminuser";
            var user = new User { UserName = login, Email = "adminuser@example.com" };
            var roles = new List<string> { "Admin" };
            var adminResponse = new AdminUserResponse { Email = "adminuser@example.com" };
            userServiceMock.Setup(a => a.GetUserByLoginAsync(login, CancellationToken.None)).ReturnsAsync(user);
            mapperMock.Setup(m => m.Map<AdminUserResponse>(user)).Returns(adminResponse);
            userServiceMock.Setup(a => a.GetUserRolesAsync(user, CancellationToken.None)).ReturnsAsync(roles);
            // Act
            var result = await getUserByInfoQueryHandler.Handle(new GetUserByInfoQuery(login), CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Email, Is.EqualTo("adminuser@example.com"));
            Assert.That(result.Roles, Is.EqualTo(roles));
        }
        [Test]
        public async Task Handle_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var login = "nonexistentuser";
            userServiceMock.Setup(a => a.GetUserByLoginAsync(login, CancellationToken.None)).ReturnsAsync((User)null);
            // Act
            var result = await getUserByInfoQueryHandler.Handle(new GetUserByInfoQuery(login), CancellationToken.None);
            // Assert
            Assert.IsNull(result);
        }
    }
}