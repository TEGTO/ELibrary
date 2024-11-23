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
        [TestCase("test-user", true, 1, Description = "Returns a cart with books when includeBooks is true.")]
        [TestCase("test-user", false, 0, Description = "Returns a cart without books when includeBooks is false.")]
        [TestCase("non-existing-user", true, 0, Description = "Returns null when the cart does not exist.")]
        public async Task GetCartByUserIdAsync_TestCases(string userId, bool includeBooks, int expectedBookCount)
        {
            // Arrange
            Cart? cart = null;
            if (userId != "non-existing-user")
            {
                cart = new Cart { UserId = userId, Books = includeBooks ? [new CartBook { BookId = 1 }] : [] };

            }

            var carts = GetDbSetMock(cart == null ? new List<Cart>() : new List<Cart> { cart });
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);

            // Act
            var result = await cartRepository.GetCartByUserIdAsync(userId, includeBooks, CancellationToken.None);

            // Assert
            if (cart == null)
            {
                Assert.IsNull(result);
            }
            else
            {
                Assert.IsNotNull(result);
                Assert.That(result.UserId, Is.EqualTo(userId));
                Assert.That(result.Books.Count, Is.EqualTo(expectedBookCount));
            }
        }

        [Test]
        [TestCase("user-1", Description = "Adds a cart with User ID 'user-1'.")]
        [TestCase("user-2", Description = "Adds a cart with User ID 'user-2'.")]
        public async Task AddCartAsync_ValidCart_AddsNewCart(string userId)
        {
            // Arrange
            var cart = new Cart { UserId = userId };
            mockRepository.Setup(r => r.AddAsync(cart, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(cart);

            // Act
            var result = await cartRepository.AddCartAsync(cart, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserId, Is.EqualTo(userId));

            mockRepository.Verify(r => r.AddAsync(cart, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [TestCase("cart-1", "cartbook-1", 1, true, Description = "Returns a CartBook when it exists in the cart.")]
        [TestCase("cart-1", "cartbook-99", 0, false, Description = "Returns null when the CartBook does not exist.")]
        public async Task GetCartBookByIdAsync_TestCases(string cartId, string bookId, int bookNumber, bool shouldExist)
        {
            // Arrange
            var cartBooks = shouldExist
                ? new List<CartBook> { new CartBook { CartId = cartId, Id = bookId, BookId = bookNumber } }
                : new List<CartBook>();

            var carts = GetDbSetMock(new List<Cart>
            {
                new Cart { Id = cartId, Books = cartBooks }
            });

            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);

            // Act
            var result = await cartRepository.GetCartBookByIdAsync(cartId, bookId, CancellationToken.None);

            // Assert
            if (shouldExist)
            {
                Assert.IsNotNull(result);
                Assert.That(result!.BookId, Is.EqualTo(bookNumber));
            }
            else
            {
                Assert.IsNull(result);
            }
        }

        [Test]
        [TestCase("cart-1", 1, true, Description = "Returns a CartBook when a book with the given ID exists in the cart.")]
        [TestCase("cart-1", 999, false, Description = "Returns null when no book with the given ID exists in the cart.")]
        public async Task GetCartBookByBookIdAsync_TestCases(string cartId, int bookId, bool shouldExist)
        {
            // Arrange
            var cartBooks = shouldExist
                ? new List<CartBook> { new CartBook { CartId = cartId, BookId = bookId } }
                : new List<CartBook>();

            var carts = GetDbSetMock(new List<Cart>
            {
                new Cart { Id = cartId, Books = cartBooks }
            });

            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);

            // Act
            var result = await cartRepository.GetCartBookByBookIdAsync(cartId, bookId, CancellationToken.None);

            // Assert
            if (shouldExist)
            {
                Assert.IsNotNull(result);
                Assert.That(result!.BookId, Is.EqualTo(bookId));
            }
            else
            {
                Assert.IsNull(result);
            }
        }

        [Test]
        [TestCase("cart-1", "book-1", 1, Description = "Updates an existing CartBook and verifies the updated properties.")]
        public async Task UpdateCartBookAsync_ValidBook_UpdatesCartBook(string cartId, string bookId, int bookNumber)
        {
            // Arrange
            var cartBook = new CartBook { CartId = cartId, Id = bookId, BookId = bookNumber };
            mockRepository.Setup(r => r.UpdateAsync(cartBook, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(cartBook);

            // Act
            var result = await cartRepository.UpdateCartBookAsync(cartBook, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(bookId));
            Assert.That(result.BookId, Is.EqualTo(bookNumber));

            mockRepository.Verify(r => r.UpdateAsync(cartBook, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [TestCase("cart-1", "user-1", Description = "Updates a valid cart with user ID 'user-1'.")]
        [TestCase("cart-2", "user-2", Description = "Updates a valid cart with user ID 'user-2'.")]
        public async Task UpdateCartAsync_ValidUpdatedCart_UpdatesCart(string cartId, string userId)
        {
            // Arrange
            var cart = new Cart { Id = cartId, UserId = userId };
            mockRepository.Setup(r => r.UpdateAsync(cart, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(cart);

            // Act
            var result = await cartRepository.UpdateCartAsync(cart, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserId, Is.EqualTo(userId));

            mockRepository.Verify(r => r.UpdateAsync(cart, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [TestCase("cart-1", "book-1", 1, Description = "Deletes an existing CartBook and verifies the call.")]
        public async Task DeleteCartBookAsync_ValidBook_DeletesCartBook(string cartId, string bookId, int bookNumber)
        {
            // Arrange
            var cartBook = new CartBook { CartId = cartId, Id = bookId, BookId = bookNumber };

            // Act
            await cartRepository.DeleteCartBookAsync(cartBook, CancellationToken.None);

            // Assert
            mockRepository.Verify(r => r.DeleteAsync(cartBook, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [TestCase("cart-1", "book-1", true, Description = "Returns true when the book exists in the cart.")]
        [TestCase("cart-1", "non-existing-book", false, Description = "Returns false when the book does not exist in the cart.")]
        public async Task CheckBookInCartAsync_TestCases(string cartId, string bookId, bool expectedResult)
        {
            // Arrange
            var cart = new Cart
            {
                Id = cartId,
                Books = expectedResult ? new List<CartBook> { new CartBook { Id = bookId, CartId = cartId } } : new List<CartBook>()
            };

            var carts = GetDbSetMock(new List<Cart> { cart });
            mockRepository.Setup(r => r.GetQueryableAsync<Cart>(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(carts);

            // Act
            var result = await cartRepository.CheckBookInCartAsync(cartId, bookId, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}