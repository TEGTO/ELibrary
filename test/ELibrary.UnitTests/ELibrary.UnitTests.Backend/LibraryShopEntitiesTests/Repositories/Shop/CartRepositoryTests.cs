using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using MockQueryable.Moq;
using Moq;
using Shared.Repositories;

namespace LibraryShopEntities.Repositories.Shop.Tests
{
    [TestFixture]
    internal class CartRepositoryTests
    {
        private Mock<IDatabaseRepository<ShopDbContext>> mockRepository;
        private CartRepository cartRepository;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new Mock<IDatabaseRepository<ShopDbContext>>();
            cartRepository = new CartRepository(mockRepository.Object);
        }

        private static IQueryable<T> GetDbSetMock<T>(List<T> data) where T : class
        {
            return data.AsQueryable().BuildMock();
        }

        [Test]
        public async Task GetCartByUserIdAsync_IncludeBooks_ReturnsCartWithBooks()
        {
            // Arrange
            var userId = "test-user";
            var cart = new Cart { UserId = userId, Books = new List<CartBook> { new CartBook { BookId = 1 } } };
            var carts = GetDbSetMock(new List<Cart> { cart });
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartRepository.GetCartByUserIdAsync(userId, true, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.Books.Count, Is.EqualTo(1));
        }
        [Test]
        public async Task GetCartByUserIdAsync_NoBooks_ReturnsCartWithoutBooks()
        {
            // Arrange
            var userId = "test-user";
            var cart = new Cart { UserId = userId };
            var carts = GetDbSetMock(new List<Cart> { cart });
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartRepository.GetCartByUserIdAsync(userId, false, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.Books, Is.Empty);
        }
        [Test]
        public async Task GetCartByUserIdAsync_CartDoesNotExist_ReturnsNull()
        {
            // Arrange
            var carts = GetDbSetMock(new List<Cart>());
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartRepository.GetCartByUserIdAsync("non-existing-user", true, CancellationToken.None);
            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task AddCartAsync_AddsNewCart()
        {
            // Arrange
            var cart = new Cart { UserId = "test-user" };
            mockRepository.Setup(r => r.AddAsync(cart, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(cart);
            // Act
            var result = await cartRepository.AddCartAsync(cart, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserId, Is.EqualTo("test-user"));
            mockRepository.Verify(r => r.AddAsync(cart, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task GetCartBookByIdAsync_BookExists_ReturnsCartBook()
        {
            // Arrange
            var cartBook = new CartBook { CartId = "cart-1", Id = "cartbook-1", BookId = 1 };
            var cart = new Cart { Id = "cart-1", Books = new List<CartBook> { cartBook } };
            var carts = GetDbSetMock(new List<Cart> { cart });
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartRepository.GetCartBookByIdAsync("cart-1", "cartbook-1", CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.BookId, Is.EqualTo(1));
        }
        [Test]
        public async Task GetCartBookByIdAsync_BookDoesNotExist_ReturnsNull()
        {
            // Arrange
            var carts = GetDbSetMock(new List<Cart>());
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartRepository.GetCartBookByIdAsync("cart-1", "cartbook-1", CancellationToken.None);
            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task GetCartBookByBookIdAsync_BookExists_ReturnsCartBook()
        {
            // Arrange
            var cartBook = new CartBook { CartId = "cart-1", BookId = 1 };
            var cart = new Cart { Id = "cart-1", Books = new List<CartBook> { cartBook } };
            var carts = GetDbSetMock(new List<Cart> { cart });
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartRepository.GetCartBookByBookIdAsync("cart-1", 1, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.BookId, Is.EqualTo(1));
        }
        [Test]
        public async Task GetCartBookByBookIdIdAsync_BookDoesNotExist_ReturnsNull()
        {
            // Arrange
            var carts = GetDbSetMock(new List<Cart>());
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartRepository.GetCartBookByBookIdAsync("cart-1", 999, CancellationToken.None);
            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task UpdateCartAsync_UpdatesCart()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1", UserId = "test-user" };
            mockRepository.Setup(r => r.UpdateAsync(cart, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(cart);
            // Act
            var result = await cartRepository.UpdateCartAsync(cart, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserId, Is.EqualTo("test-user"));
            mockRepository.Verify(r => r.UpdateAsync(cart, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task CheckBookInCartAsync_BookExistsInCart_ReturnsTrue()
        {
            // Arrange
            var cartBook = new CartBook { Id = "book-1", CartId = "cart-1" };
            var cart = new Cart { Id = "cart-1", Books = new List<CartBook> { cartBook } };
            var carts = GetDbSetMock(new List<Cart> { cart });
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartRepository.CheckBookInCartAsync("cart-1", "book-1", CancellationToken.None);
            // Assert
            Assert.IsTrue(result);
        }
        [Test]
        public async Task CheckBookInCartAsync_BookDoesNotExistInCart_ReturnsFalse()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1", Books = new List<CartBook>() };
            var carts = GetDbSetMock(new List<Cart> { cart });
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartRepository.CheckBookInCartAsync("cart-1", "non-existing-book", CancellationToken.None);
            // Assert
            Assert.IsFalse(result);
        }
    }
}