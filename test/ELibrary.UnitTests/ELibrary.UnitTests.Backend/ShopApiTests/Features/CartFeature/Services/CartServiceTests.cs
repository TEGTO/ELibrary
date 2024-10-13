using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.Extensions.Configuration;
using MockQueryable.Moq;
using Moq;
using Shared.Repositories;
using ShopApi;
using ShopApi.Features.CartFeature.Services;

namespace ShopApiTests.Features.CartFeature.Services
{
    [TestFixture]
    internal class CartServiceTests
    {
        private Mock<IDatabaseRepository<LibraryShopDbContext>> mockRepository;
        private CartService cartService;
        private Mock<IConfiguration> mockConfiguration;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new Mock<IDatabaseRepository<LibraryShopDbContext>>();
            mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c[It.Is<string>(s => s == Configuration.SHOP_MAX_ORDER_AMOUNT)])
                             .Returns("10");

            cartService = new CartService(mockRepository.Object, mockConfiguration.Object);
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
            var cart = new Cart { UserId = userId, Books = new List<CartBook> { new CartBook { BookId = 1 } } };
            var carts = GetDbSetMock(new List<Cart> { cart });

            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartService.GetCartByUserIdAsync(userId, true, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.Books.Count, Is.EqualTo(1));
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
            var result = await cartService.GetCartByUserIdAsync("non-existing-user", true, CancellationToken.None);
            // Assert
            Assert.IsNull(result);
        }
        [Test]
        public async Task GetInCartAmountAsync_CartHasMultipleBooks_ReturnsCorrectTotalAmount()
        {
            // Arrange
            var cart = new Cart
            {
                Id = "cart-1",
                Books = new List<CartBook>
                {
                    new CartBook { BookId = 1, BookAmount = 2 },
                    new CartBook { BookId = 2, BookAmount = 3 }
                }
            };
            var carts = GetDbSetMock(new List<Cart> { cart });
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var totalAmount = await cartService.GetInCartAmountAsync(cart, CancellationToken.None);
            // Assert
            Assert.That(totalAmount, Is.EqualTo(5));  // 2 + 3 = 5
        }
        [Test]
        public async Task GetInCartAmountAsync_CartHasNoBooks_ReturnsZero()
        {
            // Arrange
            var cart = new Cart
            {
                Id = "cart-1",
                Books = new List<CartBook>()
            };
            var carts = GetDbSetMock(new List<Cart> { cart });
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var totalAmount = await cartService.GetInCartAmountAsync(cart, CancellationToken.None);
            // Assert
            Assert.That(totalAmount, Is.EqualTo(0));
        }

        [Test]
        public async Task GetInCartAmountAsync_MultipleCarts_ReturnsCorrectSumForEachCart()
        {
            // Arrange
            var cart1 = new Cart
            {
                Id = "cart-1",
                Books = new List<CartBook>
                {
                    new CartBook { BookId = 1, BookAmount = 2 },
                    new CartBook { BookId = 2, BookAmount = 3 }
                }
            };
            var cart2 = new Cart
            {
                Id = "cart-2",
                Books = new List<CartBook>
                {
                    new CartBook { BookId = 3, BookAmount = 5 }
                }
            };
            var carts = GetDbSetMock(new List<Cart> { cart1, cart2 });
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var totalAmount = await cartService.GetInCartAmountAsync(cart1, CancellationToken.None);
            // Assert
            Assert.That(totalAmount, Is.EqualTo(5));
        }
        [Test]
        public async Task GetInCartAmountAsync_CartDoesNotExist_ReturnsZero()
        {
            // Arrange
            var carts = GetDbSetMock(new List<Cart>());
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            var cart = new Cart { Id = "non-existing-cart" };
            // Act
            var totalAmount = await cartService.GetInCartAmountAsync(cart, CancellationToken.None);
            // Assert
            Assert.That(totalAmount, Is.EqualTo(0));
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
        public async Task AddCartBookAsync_CartBookDoesNotExist_AddsNewCartBook_And_Includes_Related_Entities()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1", Books = new List<CartBook>() };
            var cartBook = new CartBook { BookId = 1, CartId = cart.Id, Book = new Book { Id = 1, Author = new Author(), Publisher = new Publisher(), Genre = new Genre() } };
            var carts = GetDbSetMock(new List<Cart> { cart });
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            mockRepository.Setup(r => r.UpdateAsync(cart, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(cart);
            // Act
            var result = await cartService.AddCartBookAsync(cart, cartBook, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.BookId, Is.EqualTo(cartBook.BookId));
            Assert.IsNotNull(result.Book);
            Assert.IsNotNull(result.Book.Author);
            Assert.IsNotNull(result.Book.Publisher);
            Assert.IsNotNull(result.Book.Genre);
            mockRepository.Verify(r => r.UpdateAsync(cart, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task AddCartBookAsync_CartBookAlreadyExists_UpdatesExistingCartBook_And_Includes_Related_Entities()
        {
            // Arrange
            var cartBook = new CartBook
            {
                BookId = 1,
                BookAmount = 1,
                CartId = "cart-1",
                Book = new Book { Id = 1, Author = new Author(), Publisher = new Publisher(), Genre = new Genre() }
            };
            var cart = new Cart { Id = "cart-1", Books = new List<CartBook> { cartBook } };
            var carts = GetDbSetMock(new List<Cart> { cart });
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);
            // Act
            var result = await cartService.AddCartBookAsync(cart, new CartBook { BookId = 1, BookAmount = 1 }, CancellationToken.None);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.BookId, Is.EqualTo(cartBook.BookId));
            Assert.That(result.BookAmount, Is.EqualTo(2));
            Assert.IsNotNull(result.Book);
            Assert.IsNotNull(result.Book.Author);
            Assert.IsNotNull(result.Book.Publisher);
            Assert.IsNotNull(result.Book.Genre);
            mockRepository.Verify(r => r.UpdateAsync(cartBook, It.IsAny<CancellationToken>()), Times.Once);
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
            Assert.That(ex.Message, Is.EqualTo("Cart is not found."));
        }
        [Test]
        public async Task UpdateCartBookAsync_CartBookExists_UpdatesCartBook_And_Includes_Related_Entities()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1" };
            var cartBook = new CartBook { Id = "cart-book-1", CartId = cart.Id, BookId = 1, Book = new Book { Id = 1, Author = new Author(), Publisher = new Publisher(), Genre = new Genre() } };
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
            Assert.IsNotNull(result.Book);
            Assert.IsNotNull(result.Book.Author);
            Assert.IsNotNull(result.Book.Publisher);
            Assert.IsNotNull(result.Book.Genre);
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
            Assert.That(ex.Message, Is.EqualTo("CartBook is not found or does not belong to the specified cart."));
        }
        [Test]
        public async Task DeleteCartBooksFromCartAsync_CartExists_DeletesCartBooks()
        {
            // Arrange
            var cart = new Cart
            {
                Id = "cart-1",
                Books = new List<CartBook>
                {
                    new CartBook { Id = "cart-book-1", BookId = 1 },
                    new CartBook { Id = "cart-book-2" }
                }
            };
            var booksToDelete = new[]
            {
                new Book { Id = 1 }
            };
            var cartQueryable = new List<Cart> { cart }.AsQueryable().BuildMock();
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>())).ReturnsAsync(cartQueryable);
            // Act
            var result = await cartService.DeleteBooksFromCartAsync(cart, booksToDelete, It.IsAny<CancellationToken>());
            // Assert
            Assert.That(result.Books.Count, Is.EqualTo(1));
            Assert.That(result.Books.First().Id, Is.EqualTo("cart-book-2"));
            mockRepository.Verify(r => r.DeleteAsync(It.Is<CartBook>(cb => cb.Id == "cart-book-1"), It.IsAny<CancellationToken>()), Times.Once);
            mockRepository.Verify(r => r.UpdateAsync(cart, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task DeleteCartBooksFromCartAsync_CartDoesNotExist_ReturnsNull()
        {
            // Arrange
            var cart = new Cart { Id = "non-existing-cart" };
            var booksToDelete = new[]
            {
                new Book { Id = 1 }
            };
            var cartQueryable = new List<Cart>().AsQueryable().BuildMock();
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>())).ReturnsAsync(cartQueryable);
            // Act
            var result = await cartService.DeleteBooksFromCartAsync(cart, booksToDelete, It.IsAny<CancellationToken>());
            // Assert
            Assert.IsNull(result);
            mockRepository.Verify(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()), Times.Once);
            mockRepository.Verify(r => r.DeleteAsync(It.IsAny<CartBook>(), It.IsAny<CancellationToken>()), Times.Never);
            mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        [Test]
        public async Task DeleteCartBooksFromCartAsync_NoMatchingBooksInCart_DoesNotDelete()
        {
            // Arrange
            var cart = new Cart
            {
                Id = "cart-1",
                Books = new List<CartBook>
                {
                    new CartBook { Id = "cart-book-1" }
                }
            };
            var booksToDelete = new[]
            {
                new Book { Id = 1 }
            };
            var cartQueryable = new List<Cart> { cart }.AsQueryable().BuildMock();
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>())).ReturnsAsync(cartQueryable);
            // Act
            var result = await cartService.DeleteBooksFromCartAsync(cart, booksToDelete, It.IsAny<CancellationToken>());
            // Assert
            Assert.That(result.Books.Count, Is.EqualTo(1));
            Assert.That(result.Books.First().Id, Is.EqualTo("cart-book-1"));
            mockRepository.Verify(r => r.DeleteAsync(It.IsAny<CartBook>(), It.IsAny<CancellationToken>()), Times.Never);
            mockRepository.Verify(r => r.UpdateAsync(cart, It.IsAny<CancellationToken>()), Times.Never);
        }
        [Test]
        public async Task DeleteCartBooksFromCartAsync_CartHasNoBooks_DoesNotDelete()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1", Books = new List<CartBook>() };
            var booksToDelete = new[]
            {
                new Book { Id = 1 }
            };
            var cartQueryable = new List<Cart> { cart }.AsQueryable().BuildMock();
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>())).ReturnsAsync(cartQueryable);
            // Act
            var result = await cartService.DeleteBooksFromCartAsync(cart, booksToDelete, It.IsAny<CancellationToken>());
            // Assert
            Assert.That(result.Books, Is.Empty);  // No books to delete
            mockRepository.Verify(r => r.DeleteAsync(It.IsAny<CartBook>(), It.IsAny<CancellationToken>()), Times.Never);
            mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        [Test]
        public async Task CheckBookCartAsync_ReturnsTrue_WhenCartBookExists()
        {
            // Arrange
            var cartBook = new CartBook { Id = "cart-book-1", CartId = "cart-1" };
            var cart = new Cart { Id = "cart-1", Books = new List<CartBook> { cartBook } };
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