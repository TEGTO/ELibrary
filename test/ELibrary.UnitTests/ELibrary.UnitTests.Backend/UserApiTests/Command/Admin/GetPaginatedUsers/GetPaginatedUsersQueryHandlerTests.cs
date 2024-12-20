﻿using AutoMapper;
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
        private Mock<IUserService> userServiceMock;
        private GetPaginatedUsersQueryHandler getPaginatedUsersQueryHandler;

        [SetUp]
        public void SetUp()
        {
            mapperMock = new Mock<IMapper>();
            userServiceMock = new Mock<IUserService>();

            getPaginatedUsersQueryHandler = new GetPaginatedUsersQueryHandler(userServiceMock.Object, mapperMock.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsUsersWithRoles()
        {
            // Arrange
            var users = new List<User> { new User { Email = "user1@example.com" }, new User { Email = "user2@example.com" } };
            var roles = new List<string> { "User" };

            var filter = new AdminGetUserFilter { PageNumber = 1, PageSize = 10 };
            var userResponse = new AdminUserResponse { Email = "user1@example.com", Roles = roles };

            userServiceMock.Setup(a => a.GetPaginatedUsersAsync(filter, It.IsAny<CancellationToken>())).ReturnsAsync(users);
            userServiceMock.Setup(a => a.GetUserRolesAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ReturnsAsync(roles);

            mapperMock.Setup(m => m.Map<AdminUserResponse>(It.IsAny<User>())).Returns(userResponse);

            // Act
            var result = await getPaginatedUsersQueryHandler.Handle(new GetPaginatedUsersQuery(filter), CancellationToken.None);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Email, Is.EqualTo("user1@example.com"));
        }
    }
}