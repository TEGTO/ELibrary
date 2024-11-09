using LibraryShopEntities.Domain.Entities.Shop;
using LibraryShopEntities.Repositories.Shop;
using Microsoft.Extensions.Configuration;
using Moq;
using ShopApi;
using ShopApi.Features.CartFeature.Services;

namespace ShopApiTests.Features.CartFeature.Services
{
    [TestFixture]
    internal class CartServiceTests
    {
        private Mock<ICartRepository> mockRepository;
        private Mock<IConfiguration> mockConfiguration;
        private CartService cartService;
        private const int MaxBookAmount = 10;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new Mock<ICartRepository>();
            mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c[Configuration.SHOP_MAX_ORDER_AMOUNT]).Returns(MaxBookAmount.ToString());

            cartService = new CartService(mockRepository.Object, mockConfiguration.Object);
        }

        [Test]
        public async Task GetCartByUserIdAsync_IncludeBooks_ReturnsCartWithBooks()
        {
            // Arrange
            var userId = "test-user";
            var cart = new Cart { UserId = userId, Books = new List<CartBook> { new CartBook { BookId = 1 } } };
            mockRepository.Setup(r => r.GetCartByUserIdAsync(userId, true, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(cart);
            // Act
            var result = await cartService.GetCartByUserIdAsync(userId, true, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.Books.Count, Is.EqualTo(1));
        }
        [Test]
        public async Task GetInCartAmountAsync_ReturnsTotalBookAmountInCart()
        {
            // Arrange
            var cart = new Cart { UserId = "test-user", Books = new List<CartBook> { new CartBook { BookId = 1, BookAmount = 3 }, new CartBook { BookId = 2, BookAmount = 2 } } };
            mockRepository.Setup(r => r.GetCartByUserIdAsync(cart.UserId, true, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(cart);
            // Act
            var totalAmount = await cartService.GetInCartAmountAsync(cart, CancellationToken.None);
            // Assert
            Assert.That(totalAmount, Is.EqualTo(5));
        }
        [Test]
        public async Task CreateCartAsync_AddsNewCart()
        {
            // Arrange
            var userId = "test-user";
            var cart = new Cart { UserId = userId };
            mockRepository.Setup(r => r.AddCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>())).ReturnsAsync(cart);
            // Act
            var result = await cartService.CreateCartAsync(userId, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserId, Is.EqualTo(userId));
            mockRepository.Verify(r => r.AddCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task AddCartBookAsync_NewBookUnderMaxAmount_AddsBook()
        {
            // Arrange
            var cart = new Cart { UserId = "test-user", Books = new List<CartBook>() };
            var cartBook = new CartBook { BookId = 1, BookAmount = 3 };
            mockRepository.Setup(r => r.GetCartByUserIdAsync(cart.UserId, true, It.IsAny<CancellationToken>())).ReturnsAsync(cart);
            mockRepository.Setup(r => r.UpdateCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>())).ReturnsAsync(cart);
            mockRepository.Setup(r => r.GetCartBookByBookIdAsync(cart.Id, cartBook.BookId, It.IsAny<CancellationToken>())).ReturnsAsync(cartBook);
            // Act
            var result = await cartService.AddCartBookAsync(cart, cartBook, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.BookId, Is.EqualTo(cartBook.BookId));
            mockRepository.Verify(r => r.UpdateCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void AddCartBookAsync_ExceedsMaxBookAmount_ThrowsException()
        {
            // Arrange
            var cart = new Cart { UserId = "test-user", Books = new List<CartBook> { new CartBook { BookId = 1, BookAmount = MaxBookAmount } } };
            var cartBook = new CartBook { BookId = 1, BookAmount = 1 };
            mockRepository.Setup(r => r.GetCartByUserIdAsync(cart.UserId, true, It.IsAny<CancellationToken>())).ReturnsAsync(cart);
            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => cartService.AddCartBookAsync(cart, cartBook, CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("Failed to add or update cart book."));
        }
        [Test]
        public async Task UpdateCartBookAsync_UpdatesExistingCartBook()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1" };
            var cartBook = new CartBook { BookId = 1, BookAmount = 3 };
            mockRepository.Setup(r => r.GetCartBookByIdAsync(cart.Id, cartBook.Id, It.IsAny<CancellationToken>())).ReturnsAsync(cartBook);
            mockRepository.Setup(r => r.UpdateCartBookAsync(cartBook, It.IsAny<CancellationToken>())).ReturnsAsync(cartBook);
            // Act
            var result = await cartService.UpdateCartBookAsync(cart, cartBook, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.BookId, Is.EqualTo(cartBook.BookId));
        }
        [Test]
        public async Task DeleteBooksFromCartAsync_RemovesSpecifiedBooks()
        {
            // Arrange
            var cart = new Cart
            {
                UserId = "test-user",
                Books = new List<CartBook> { new CartBook { BookId = 1 }, new CartBook { BookId = 2 } }
            };
            var bookIdsToDelete = new[] { 1 };
            mockRepository.Setup(r => r.GetCartByUserIdAsync(cart.UserId, true, It.IsAny<CancellationToken>())).ReturnsAsync(cart);
            // Act
            var updatedCart = await cartService.DeleteBooksFromCartAsync(cart, bookIdsToDelete, CancellationToken.None);
            // Assert
            Assert.That(updatedCart.Books.Count, Is.EqualTo(1));
            Assert.That(updatedCart.Books.Any(b => b.BookId == 1), Is.False);
            mockRepository.Verify(r => r.DeleteCartBookAsync(It.Is<CartBook>(cb => cb.BookId == 1), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task CheckBookCartAsync_ReturnsTrue_WhenBookExists()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1" };
            var bookId = "book-1";
            mockRepository.Setup(r => r.CheckBookInCartAsync(cart.Id, bookId, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            // Act
            var result = await cartService.CheckBookCartAsync(cart, bookId, CancellationToken.None);
            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task CheckBookCartAsync_ReturnsFalse_WhenBookDoesNotExist()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1" };
            var bookId = "non-existing-book";
            mockRepository.Setup(r => r.CheckBookInCartAsync(cart.Id, bookId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            // Act
            var result = await cartService.CheckBookCartAsync(cart, bookId, CancellationToken.None);
            // Assert
            Assert.IsFalse(result);
        }
    }
}