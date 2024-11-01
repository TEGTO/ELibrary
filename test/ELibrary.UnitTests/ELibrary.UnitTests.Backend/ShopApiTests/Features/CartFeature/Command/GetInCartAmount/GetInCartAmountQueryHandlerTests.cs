using AutoMapper;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.CartFeature.Services;

namespace ShopApi.Features.CartFeature.Command.GetInCartAmount.Tests
{
    [TestFixture]
    internal class GetInCartAmountQueryHandlerTests
    {
        private Mock<ICartService> mockCartService;
        private Mock<IMapper> mockMapper;
        private GetInCartAmountQueryHandler handler;

        [SetUp]
        public void SetUp()
        {
            mockCartService = new Mock<ICartService>();
            mockMapper = new Mock<IMapper>();
            handler = new GetInCartAmountQueryHandler(mockCartService.Object, mockMapper.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnInCartAmount_WhenCartExists()
        {
            // Arrange
            var query = new GetInCartAmountQuery("user123");
            var cancellationToken = CancellationToken.None;
            var existingCart = new Cart { Id = "existingCartId" };
            var expectedAmount = 5;
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(query.UserId, true, cancellationToken))
                .ReturnsAsync(existingCart);
            mockCartService.Setup(cs => cs.GetInCartAmountAsync(existingCart, cancellationToken))
                .ReturnsAsync(expectedAmount);
            // Act
            var result = await handler.Handle(query, cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(expectedAmount));
            mockCartService.Verify(cs => cs.GetCartByUserIdAsync(query.UserId, true, cancellationToken), Times.Once);
            mockCartService.Verify(cs => cs.GetInCartAmountAsync(existingCart, cancellationToken), Times.Once);
        }
        [Test]
        public async Task Handle_ShouldCreateCartAndReturnInCartAmount_WhenCartDoesNotExist()
        {
            // Arrange
            var query = new GetInCartAmountQuery("user123");
            var cancellationToken = CancellationToken.None;
            var newCart = new Cart { Id = "newCartId" };
            var expectedAmount = 3;
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(query.UserId, true, cancellationToken))
                .ReturnsAsync((Cart)null);
            mockCartService.Setup(cs => cs.CreateCartAsync(query.UserId, cancellationToken))
                .ReturnsAsync(newCart);
            mockCartService.Setup(cs => cs.GetInCartAmountAsync(newCart, cancellationToken))
                .ReturnsAsync(expectedAmount);
            // Act
            var result = await handler.Handle(query, cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(expectedAmount));
            mockCartService.Verify(cs => cs.GetCartByUserIdAsync(query.UserId, true, cancellationToken), Times.Once);
            mockCartService.Verify(cs => cs.CreateCartAsync(query.UserId, cancellationToken), Times.Once);
            mockCartService.Verify(cs => cs.GetInCartAmountAsync(newCart, cancellationToken), Times.Once);
        }
    }
}