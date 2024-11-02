using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.CartFeature.Dtos;
using ShopApi.Features.CartFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.CartFeature.Command.DeleteBooksFromCart.Tests
{
    [TestFixture]
    internal class DeleteBooksFromCartCommandHandlerTests
    {
        private Mock<ICartService> mockCartService;
        private Mock<ILibraryService> mockLibraryService;
        private Mock<IMapper> mockMapper;
        private DeleteBooksFromCartCommandHandler handler;

        [SetUp]
        public void SetUp()
        {
            mockCartService = new Mock<ICartService>();
            mockLibraryService = new Mock<ILibraryService>();
            mockMapper = new Mock<IMapper>();
            handler = new DeleteBooksFromCartCommandHandler(mockCartService.Object, mockLibraryService.Object, mockMapper.Object);
        }

        [Test]
        public async Task Handle_ShouldCreateCartIfNoneExists()
        {
            // Arrange
            var command = new DeleteBooksFromCartCommand("user123", new[] { new DeleteCartBookFromCartRequest { Id = 1 } });
            var cancellationToken = CancellationToken.None;
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), false, cancellationToken))
                .ReturnsAsync((Cart)null);
            mockCartService.Setup(cs => cs.CreateCartAsync(It.IsAny<string>(), cancellationToken))
                .ReturnsAsync(new Cart { Id = "newCartId" });
            mockMapper.Setup(m => m.Map<Book>(It.IsAny<DeleteCartBookFromCartRequest>()))
              .Returns(new Book { Id = 1 });
            mockLibraryService.Setup(s => s.GetByIdsAsync<BookResponse>(
               It.IsAny<List<int>>(),
               It.IsAny<string>(),
               cancellationToken)
           ).ReturnsAsync(new List<BookResponse> { new BookResponse { Id = 1, Name = "Mapped Book" } });
            var cartResponse = new CartResponse { Books = new List<CartBookResponse> { new CartBookResponse { BookId = 1 } } };
            mockMapper.Setup(m => m.Map<CartResponse>(It.IsAny<Cart>())).Returns(cartResponse);
            // Act
            await handler.Handle(command, cancellationToken);
            // Assert
            mockCartService.Verify(cs => cs.CreateCartAsync(command.UserId, cancellationToken), Times.Once);
        }
        [Test]
        public async Task Handle_ShouldDeleteBooksFromCart()
        {
            // Arrange
            var command = new DeleteBooksFromCartCommand("user123", new[] { new DeleteCartBookFromCartRequest { Id = 1 } });
            var cart = new Cart { Id = "existingCartId" };
            var cancellationToken = CancellationToken.None;
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), false, cancellationToken))
                .ReturnsAsync(cart);
            mockMapper.Setup(m => m.Map<Book>(It.IsAny<DeleteCartBookFromCartRequest>()))
                .Returns(new Book { Id = 1 });
            mockLibraryService.Setup(s => s.GetByIdsAsync<BookResponse>(
                It.IsAny<List<int>>(),
                It.IsAny<string>(),
                cancellationToken)
            ).ReturnsAsync(new List<BookResponse> { new BookResponse { Id = 1, Name = "Sample Book" } });
            mockCartService.Setup(cs => cs.DeleteBooksFromCartAsync(It.IsAny<Cart>(), It.IsAny<int[]>(), cancellationToken))
                .ReturnsAsync(cart);
            var cartResponse = new CartResponse { Books = new List<CartBookResponse> { new CartBookResponse { BookId = 1 } } };
            mockMapper.Setup(m => m.Map<CartResponse>(It.IsAny<Cart>())).Returns(cartResponse);
            // Act
            var result = await handler.Handle(command, cancellationToken);
            // Assert
            mockCartService.Verify(cs => cs.DeleteBooksFromCartAsync(cart, It.IsAny<int[]>(), cancellationToken), Times.Once);
            Assert.NotNull(result);
            Assert.That(result.Books.First().BookId, Is.EqualTo(1));
            Assert.That(result.Books.First().Book.Name, Is.EqualTo("Sample Book"));
        }
        [Test]
        public async Task Handle_ShouldMapAndReturnCartResponse()
        {
            // Arrange
            var command = new DeleteBooksFromCartCommand("user123", new[] { new DeleteCartBookFromCartRequest { Id = 2 } });
            var cart = new Cart { Id = "cartId" };
            var cancellationToken = CancellationToken.None;
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), false, cancellationToken))
                .ReturnsAsync(cart);
            mockMapper.Setup(m => m.Map<Book>(It.IsAny<DeleteCartBookFromCartRequest>()))
                .Returns(new Book { Id = 2 });
            mockLibraryService.Setup(s => s.GetByIdsAsync<BookResponse>(
                It.IsAny<List<int>>(),
                It.IsAny<string>(),
                cancellationToken)
            ).ReturnsAsync(new List<BookResponse> { new BookResponse { Id = 2, Name = "Mapped Book" } });
            mockCartService.Setup(cs => cs.DeleteBooksFromCartAsync(cart, It.IsAny<int[]>(), cancellationToken))
                .ReturnsAsync(cart);
            var cartResponse = new CartResponse { Books = new List<CartBookResponse> { new CartBookResponse { BookId = 2 } } };
            mockMapper.Setup(m => m.Map<CartResponse>(cart)).Returns(cartResponse);
            // Act
            var result = await handler.Handle(command, cancellationToken);
            // Assert
            Assert.NotNull(result);
            Assert.That(result.Books.First().BookId, Is.EqualTo(2));
            Assert.That(result.Books.First().Book.Name, Is.EqualTo("Mapped Book"));
        }
    }
}