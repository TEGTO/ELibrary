using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.CartFeature.Dtos;
using ShopApi.Features.CartFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.CartFeature.Command.AddBookToCart.Tests
{
    [TestFixture]
    internal class AddBookToCartCommandHandlerTests
    {
        private Mock<ICartService> mockCartService;
        private Mock<ILibraryService> mockLibraryService;
        private Mock<IMapper> mockMapper;
        private AddBookToCartCommandHandler handler;

        [SetUp]
        public void SetUp()
        {
            mockCartService = new Mock<ICartService>();
            mockLibraryService = new Mock<ILibraryService>();
            mockMapper = new Mock<IMapper>();
            handler = new AddBookToCartCommandHandler(mockCartService.Object, mockLibraryService.Object, mockMapper.Object);
        }

        [Test]
        public async Task Handle_ShouldCreateCartIfNoneExists()
        {
            // Arrange
            var command = new AddBookToCartCommand("user123", new AddBookToCartRequest());
            var cancellationToken = CancellationToken.None;
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart)null);
            mockCartService.Setup(cs => cs.CreateCartAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Cart { Id = "newCartId" });
            mockMapper.Setup(m => m.Map<CartBook>(command.Request)).Returns(new CartBook { BookId = 1, BookAmount = 1 });
            mockLibraryService.Setup(s => s.GetByIdsAsync<BookResponse>(
                It.IsAny<List<int>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(new List<BookResponse> { new BookResponse { Id = 1, Name = "Sample Book" } });
            mockCartService.Setup(cs => cs.AddCartBookAsync(It.IsAny<Cart>(), It.IsAny<CartBook>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CartBook { Id = "cartBookId", BookAmount = 1, BookId = 1 });
            mockMapper.Setup(m => m.Map<CartBookResponse>(It.IsAny<CartBook>()))
                .Returns(new CartBookResponse { Id = "cartBookId", BookAmount = 1, BookId = 1 });
            // Act
            var result = await handler.Handle(command, cancellationToken);
            // Assert
            mockCartService.Verify(cs => cs.CreateCartAsync(command.UserId, cancellationToken), Times.Once);
            Assert.NotNull(result);
            Assert.That(result.Book.Name, Is.EqualTo("Sample Book"));
        }
        [Test]
        public async Task Handle_ShouldAddBookToExistingCart()
        {
            // Arrange
            var command = new AddBookToCartCommand("user123", new AddBookToCartRequest());
            var cart = new Cart { Id = "existingCartId" };
            var cancellationToken = CancellationToken.None;
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);
            var cartBook = new CartBook { BookId = 2, BookAmount = 1 };
            mockMapper.Setup(m => m.Map<CartBook>(command.Request)).Returns(cartBook);
            mockLibraryService.Setup(s => s.GetByIdsAsync<BookResponse>(
                It.IsAny<List<int>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(new List<BookResponse> { new BookResponse { Id = 2, Name = "Another Book" } });
            mockCartService.Setup(cs => cs.AddCartBookAsync(It.IsAny<Cart>(), It.IsAny<CartBook>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CartBook { Id = "cartBookId2", BookAmount = 1, BookId = 2 });
            mockMapper.Setup(m => m.Map<CartBookResponse>(It.IsAny<CartBook>()))
                .Returns(new CartBookResponse { Id = "cartBookId2", BookAmount = 1, BookId = 2, Book = new BookResponse { Name = "Another Book" } });
            // Act
            var result = await handler.Handle(command, cancellationToken);
            // Assert
            mockCartService.Verify(cs => cs.GetCartByUserIdAsync(command.UserId, false, cancellationToken), Times.Once);
            Assert.NotNull(result);
            Assert.That(result.Book.Name, Is.EqualTo("Another Book"));
        }
        [Test]
        public async Task Handle_ShouldMapAndReturnCorrectResponse()
        {
            // Arrange
            var command = new AddBookToCartCommand("user123", new AddBookToCartRequest { BookId = 3, BookAmount = 1 });
            var cart = new Cart { Id = "cartId" };
            var cancellationToken = CancellationToken.None;
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);
            mockMapper.Setup(m => m.Map<CartBook>(command.Request))
                .Returns(new CartBook { Id = "cartBookId3", BookId = 3, BookAmount = 1 });
            mockLibraryService.Setup(s => s.GetByIdsAsync<BookResponse>(
                It.IsAny<List<int>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(new List<BookResponse> { new BookResponse { Id = 3, Name = "Mapped Book" } });
            mockCartService.Setup(cs => cs.AddCartBookAsync(It.IsAny<Cart>(), It.IsAny<CartBook>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CartBook { Id = "cartBookId3", BookAmount = 1, BookId = 3 });
            mockMapper.Setup(m => m.Map<CartBookResponse>(It.IsAny<CartBook>()))
                .Returns(new CartBookResponse { Id = "cartBookId3", BookAmount = 1, BookId = 3, Book = new BookResponse { Name = "Mapped Book" } });
            // Act
            var result = await handler.Handle(command, cancellationToken);
            // Assert
            Assert.NotNull(result);
            Assert.That(result.Book.Name, Is.EqualTo("Mapped Book"));
            Assert.That(result.BookId, Is.EqualTo(3));
        }
    }
}