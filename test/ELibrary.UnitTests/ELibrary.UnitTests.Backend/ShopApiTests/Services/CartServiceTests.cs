using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using MockQueryable.Moq;
using Moq;
using Shared.Repositories;

namespace ShopApi.Services.Tests
{
    [TestFixture]
    internal class CartServiceTests
    {
        private Mock<IDatabaseRepository<LibraryShopDbContext>> mockRepository;
        private CartService cartService;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new Mock<IDatabaseRepository<LibraryShopDbContext>>();
            cartService = new CartService(mockRepository.Object);
        }

        private static IQueryable<T> GetDbSetMock<T>(List<T> data) where T : class
        {
            return data.AsQueryable().BuildMock();
        }

        [Test]
        public async Task GetCartByUserIdAsync_IncludeProductsIsTrue_ReturnsCartWithBooks()
        {
            // Arrange
            var userId = "test-user";
            var cart = new Cart { UserId = userId, CartBooks = new List<CartBook> { new CartBook { BookId = 1 } } };
            var carts = GetDbSetMock(new List<Cart> { cart });

            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartService.GetCartByUserIdAsync(userId, true, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.CartBooks.Count, Is.EqualTo(1));
        }
        [Test]
        public async Task GetCartByUserIdAsync_IncludeProductsIsFalse_ReturnsCartWithoutBooks()
        {
            // Arrange
            var userId = "test-user";
            var cart = new Cart { UserId = userId };
            var carts = GetDbSetMock(new List<Cart> { cart });

            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartService.GetCartByUserIdAsync(userId, false, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.CartBooks, Is.Empty);
        }
        [Test]
        public async Task GetCartByUserIdAsync_CartDoesNotExist_ReturnsNull()
        {
            // Arrange
            var carts = GetDbSetMock(new List<Cart>());

            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartService.GetCartByUserIdAsync("non-existing-user", true, CancellationToken.None);
            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task CreateCartAsync_CreatesNewCart()
        {
            // Arrange
            var userId = "test-user";
            var cart = new Cart { UserId = userId };

            mockRepository.Setup(r => r.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(cart);
            // Act
            var result = await cartService.CreateCartAsync(userId, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserId, Is.EqualTo(userId));
            mockRepository.Verify(r => r.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task AddCartBookAsync_CartBookDoesNotExist_AddsNewCartBook()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1", CartBooks = new List<CartBook>() };
            var cartBook = new CartBook { BookId = 1 };
            var carts = GetDbSetMock(new List<Cart> { cart });
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartService.AddCartBookAsync(cart, cartBook, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.BookId, Is.EqualTo(cartBook.BookId));
            mockRepository.Verify(r => r.UpdateAsync(cart, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void AddCartBookAsync_СartDoesNotExist_ThrowsException()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1" };
            var cartBook = new CartBook { BookId = 1 };
            var carts = GetDbSetMock(new List<Cart>());
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => cartService.AddCartBookAsync(cart, cartBook, CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("Cart not found."));
        }
        [Test]
        public async Task UpdateCartBookAsync_CartBookExists_UpdatesCartBook()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1" };
            var cartBook = new CartBook { Id = "cart-book-1", CartId = cart.Id, BookId = 1 };
            var cartBooks = GetDbSetMock(new List<CartBook> { cartBook });
            mockRepository.Setup(r => r.GetQueryableAsync<CartBook>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(cartBooks);
            mockRepository.Setup(r => r.UpdateAsync(cartBook, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(cartBook);
            // Act
            var result = await cartService.UpdateCartBookAsync(cart, cartBook, CancellationToken.None);
            // Assert
            Assert.That(result.Id, Is.EqualTo(cartBook.Id));
            Assert.That(result.BookId, Is.EqualTo(1));
            mockRepository.Verify(r => r.UpdateAsync(cartBook, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void UpdateCartBookAsync_CartBookDoesNotExist_ThrowsException()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1" };
            var cartBook = new CartBook { Id = "cart-book-1", CartId = cart.Id };
            var cartBooks = GetDbSetMock(new List<CartBook>());
            mockRepository.Setup(r => r.GetQueryableAsync<CartBook>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(cartBooks);
            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => cartService.UpdateCartBookAsync(cart, cartBook, CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("CartBook not found or does not belong to the specified cart."));
        }
        [Test]
        public async Task DeleteCartBookAsync_CartBookExists_DeletesCartBook()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1" };
            var cartBook = new CartBook { Id = "cart-book-1", CartId = cart.Id };
            var cartBooks = GetDbSetMock(new List<CartBook> { cartBook });
            mockRepository.Setup(r => r.GetQueryableAsync<CartBook>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(cartBooks);
            // Act
            await cartService.DeleteCartBookAsync(cart, cartBook.Id, CancellationToken.None);
            // Assert
            mockRepository.Verify(r => r.DeleteAsync(cartBook, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public void DeleteCartBookAsync_CartBookDoesNotExist_ThrowsException()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1" };
            var cartBooks = GetDbSetMock(new List<CartBook>());
            mockRepository.Setup(r => r.GetQueryableAsync<CartBook>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(cartBooks);
            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => cartService.DeleteCartBookAsync(cart, "non-existing-cart-book", CancellationToken.None));
            Assert.That(ex.Message, Is.EqualTo("CartBook not found or does not belong to the specified cart."));
        }
        [Test]
        public async Task ClearCartAsync_RemovesAllCartBooks()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1", CartBooks = new List<CartBook> { new CartBook { BookId = 1 } } };
            var carts = GetDbSetMock(new List<Cart> { cart });

            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartService.ClearCartAsync(cart, CancellationToken.None);
            // Assert
            Assert.That(result.CartBooks, Is.Empty);
            mockRepository.Verify(r => r.DeleteAsync(It.IsAny<CartBook>(), It.IsAny<CancellationToken>()), Times.Once);
            mockRepository.Verify(r => r.UpdateAsync(cart, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task CheckBookCartAsync_ReturnsTrue_WhenCartBookExists()
        {
            // Arrange
            var cartBook = new CartBook { Id = "cart-book-1", CartId = "cart-1" };
            var cart = new Cart { Id = "cart-1", CartBooks = new List<CartBook> { cartBook } };
            var carts = GetDbSetMock(new List<Cart> { cart });
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartService.CheckBookCartAsync(cart, cartBook.Id, CancellationToken.None);
            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task CheckBookCartAsync_ReturnsFalse_WhenCartBookDoesNotExist()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1" };
            var carts = GetDbSetMock(new List<Cart> { cart });
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartService.CheckBookCartAsync(cart, "non-existing-cart-book", CancellationToken.None);
            // Assert
            Assert.IsFalse(result);
        }
    }
}