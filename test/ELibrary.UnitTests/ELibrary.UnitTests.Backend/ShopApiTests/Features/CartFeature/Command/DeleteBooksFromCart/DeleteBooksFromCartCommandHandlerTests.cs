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

            var cartResponse = new CartResponse { Books = new List<CartBookResponse> { new CartBookResponse { BookId = 1 } } };

            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), false, cancellationToken))
                .ReturnsAsync(null as Cart);
            mockCartService.Setup(cs => cs.CreateCartAsync(It.IsAny<string>(), cancellationToken))
                .ReturnsAsync(new Cart { Id = "newCartId" });
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), true, cancellationToken))
                .ReturnsAsync(new Cart() { Books = new List<CartBook>() { new CartBook() { BookId = 1 } } });

            mockMapper.Setup(m => m.Map<Book>(It.IsAny<DeleteCartBookFromCartRequest>()))
                .Returns(new Book { Id = 1 });
            mockMapper.Setup(m => m.Map<CartResponse>(It.IsAny<Cart>()))
                .Returns(cartResponse);

            mockLibraryService.Setup(s => s.GetByIdsAsync<BookResponse>(
                It.IsAny<List<int>>(),
                It.IsAny<string>(),
                cancellationToken))
                .ReturnsAsync(new List<BookResponse> { new BookResponse { Id = 1, Name = "Mapped Book" } });

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

            var bookResponse = new BookResponse { Id = 1, Name = "Sample Book" };
            var cartResponse = new CartResponse { Books = new List<CartBookResponse> { new CartBookResponse { BookId = 1, Book = bookResponse } } };

            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), false, cancellationToken))
                .ReturnsAsync(cart);
            mockCartService.Setup(cs => cs.DeleteBooksFromCartAsync(It.IsAny<Cart>(), It.IsAny<int[]>(), cancellationToken))
                .ReturnsAsync(cart);
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), true, cancellationToken))
                .ReturnsAsync(new Cart() { Books = new List<CartBook>() { new CartBook() { BookId = 1 } } });

            mockMapper.Setup(m => m.Map<Book>(It.IsAny<DeleteCartBookFromCartRequest>()))
                .Returns(new Book { Id = 1 });
            mockMapper.Setup(m => m.Map<CartResponse>(It.IsAny<Cart>()))
                .Returns(cartResponse);

            mockLibraryService.Setup(s => s.GetByIdsAsync<BookResponse>(
                It.IsAny<List<int>>(),
                It.IsAny<string>(),
                cancellationToken))
                .ReturnsAsync(new List<BookResponse> { bookResponse });

            // Act
            var result = await handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Books);
            Assert.NotNull(result.Books[0]);
            Assert.NotNull(result.Books[0].Book);

            Assert.That(result.Books[0].BookId, Is.EqualTo(1));
            Assert.That(result.Books[0].Book.Name, Is.EqualTo("Sample Book"));

            mockCartService.Verify(cs => cs.DeleteBooksFromCartAsync(cart, It.IsAny<int[]>(), cancellationToken), Times.Once);
            mockCartService.Verify(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), false, cancellationToken), Times.Once);
            mockCartService.Verify(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), true, cancellationToken), Times.Once);

            mockLibraryService.Verify(cs => cs.GetByIdsAsync<BookResponse>(It.IsAny<IEnumerable<int>>(), It.IsAny<string>(), cancellationToken), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldMapAndReturnCartResponse()
        {
            // Arrange
            var command = new DeleteBooksFromCartCommand("user123", new[] { new DeleteCartBookFromCartRequest { Id = 2 } });
            var cart = new Cart { Id = "cartId" };
            var cancellationToken = CancellationToken.None;

            var bookResponse = new BookResponse { Id = 1, Name = "Mapped Book" };
            var cartResponse = new CartResponse { Books = new List<CartBookResponse> { new CartBookResponse { BookId = 2, Book = bookResponse } } };

            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), false, cancellationToken))
                .ReturnsAsync(cart);
            mockCartService.Setup(cs => cs.DeleteBooksFromCartAsync(cart, It.IsAny<int[]>(), cancellationToken))
                .ReturnsAsync(cart);
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), true, cancellationToken))
                .ReturnsAsync(cart);

            mockLibraryService.Setup(s => s.GetByIdsAsync<BookResponse>(
                It.IsAny<List<int>>(),
                It.IsAny<string>(),
                cancellationToken))
                .ReturnsAsync(new List<BookResponse> { bookResponse });

            mockMapper.Setup(m => m.Map<Book>(It.IsAny<DeleteCartBookFromCartRequest>()))
                .Returns(new Book { Id = 2 });
            mockMapper.Setup(m => m.Map<CartResponse>(cart))
                .Returns(cartResponse);

            // Act
            var result = await handler.Handle(command, cancellationToken);

            // Assert

            Assert.NotNull(result);
            Assert.NotNull(result.Books);
            Assert.NotNull(result.Books[0]);
            Assert.NotNull(result.Books[0].Book);

            Assert.That(result.Books[0].BookId, Is.EqualTo(2));
            Assert.That(result.Books[0].Book.Name, Is.EqualTo("Mapped Book"));

            mockMapper.Verify(cs => cs.Map<CartResponse>(It.IsAny<Cart>()), Times.Once);
        }

        [Test]
        public void Handle_ShouldDeleteBookAndThrowNullableCartException()
        {
            // Arrange
            var command = new DeleteBooksFromCartCommand("user123", new[] { new DeleteCartBookFromCartRequest { Id = 2 } });
            var cart = new Cart { Id = "cartId" };
            var cancellationToken = CancellationToken.None;

            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), false, cancellationToken))
                .ReturnsAsync(cart);
            mockCartService.Setup(cs => cs.DeleteBooksFromCartAsync(cart, It.IsAny<int[]>(), cancellationToken))
                .ReturnsAsync(cart);
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), true, cancellationToken))
                .ReturnsAsync(null as Cart);

            // Act
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(command, cancellationToken));

            // Assert

            Assert.NotNull(ex);
            Assert.NotNull(ex.Message);

            Assert.That(ex.Message, Is.EqualTo("Can not find cart after cart book update!"));

            mockCartService.Verify(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), false, cancellationToken), Times.Once);
            mockCartService.Verify(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), true, cancellationToken), Times.Once);
        }
    }
}