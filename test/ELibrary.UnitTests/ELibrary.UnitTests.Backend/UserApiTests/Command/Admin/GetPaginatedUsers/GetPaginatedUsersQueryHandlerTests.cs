using AutoMapper;
using Moq;
using UserApi.Command.Admin.GetPaginatedUsers;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;
using UserEntities.Domain.Entities;

namespace UserApiTests.Command.Admin.GetPaginatedUsers
{
    [TestFixture]
    internal class GetPaginatedUsersQueryHandlerTests
    {
        private Mock<IMapper> mapperMock;
        private Mock<IAuthService> authServiceMock;
        private GetPaginatedUsersQueryHandler getPaginatedUsersQueryHandler;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            authServiceMock = new Mock<IAuthService>();

            getPaginatedUsersQueryHandler = new GetPaginatedUsersQueryHandler(authServiceMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsUsersWithRoles()
        {
            // Arrange
            var filter = new AdminGetUserFilter { PageNumber = 1, PageSize = 10 };
            var users = new List<User> { new User { Email = "user1@example.com" }, new User { Email = "user2@example.com" } };
            var roles = new List<string> { "User" };
            var userResponse = new AdminUserResponse { Email = "user1@example.com", Roles = roles };
            authServiceMock.Setup(a => a.GetPaginatedUsersAsync(filter, It.IsAny<CancellationToken>())).ReturnsAsync(users);
            mapperMock.Setup(m => m.Map<AdminUserResponse>(It.IsAny<User>())).Returns(userResponse);
            authServiceMock.Setup(a => a.GetUserRolesAsync(It.IsAny<User>())).ReturnsAsync(roles);
            // Act
            var result = await getPaginatedUsersQueryHandler.Handle(new GetPaginatedUsersQuery(filter), CancellationToken.None);
            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Email, Is.EqualTo("user1@example.com"));
        }
    }
}