using Moq;
using UserApi.Command.Admin.GetPaginatedUserTotalAmount;
using UserApi.Domain.Dtos;
using UserApi.Services;

namespace UserApiTests.Command.Admin.GetPaginatedUserTotalAmount
{
    [TestFixture]
    internal class GetPaginatedUserTotalAmountQueryHandlerTests
    {
        private Mock<IAuthService> authServiceMock;
        private GetPaginatedUserTotalAmountQueryHandler getPaginatedUserTotalAmountQueryHandler;

        [SetUp]
        public void SetUp()
        {
            authServiceMock = new Mock<IAuthService>();

            getPaginatedUserTotalAmountQueryHandler = new GetPaginatedUserTotalAmountQueryHandler(authServiceMock.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ReturnsTotalCount()
        {
            // Arrange
            var filter = new AdminGetUserFilter { PageNumber = 1, PageSize = 10 };
            var totalCount = 100;
            authServiceMock.Setup(a => a.GetUserTotalAmountAsync(filter, It.IsAny<CancellationToken>())).ReturnsAsync(totalCount);
            // Act
            var result = await getPaginatedUserTotalAmountQueryHandler.Handle(new GetPaginatedUserTotalAmountQuery(filter), CancellationToken.None);
            // Assert
            Assert.That(result, Is.EqualTo(totalCount));
        }
    }
}