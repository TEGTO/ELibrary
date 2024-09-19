using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopApi.Domain.Dtos.Cart;
using ShopApi.Services;
using System.Security.Claims;

namespace ShopApi.Controllers.Tests
{
    [TestFixture]
    [TestFixture]
    internal class CartControllerTests
    {
        private Mock<IMapper> mockMapper;
        private Mock<ICartService> mockCartService;
        private CartController cartController;

        [SetUp]
        public void SetUp()
        {
            mockMapper = new Mock<IMapper>();
            mockCartService = new Mock<ICartService>();
            cartController = new CartController(mockMapper.Object, null, mockCartService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
            }, "mock"));

            cartController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Test]
        public async Task GetCart_ValidRequest_ReturnsCartResponseWithBooks()
        {
            // Arrange
            var cart = new Cart
            {
                Id = "cart-1",
                CartBooks = new List<CartBook>
                {
                    new CartBook { Id = "book-1", BookAmount = 2, BookId = 1, Book = new Book() }
                }
            };
            var cartResponse = new CartResponse
            {
                CartBooks = new List<BookListingResponse>
                {
                    new BookListingResponse { Id = "book-1", BookAmount = 2, BookId = 1, Book = new BookResponse() }
                }
            };
            mockCartService.Setup(c => c.GetCartByUserIdAsync(It.IsAny<string>(), true, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(cart);
            mockMapper.Setup(m => m.Map<CartResponse>(cart)).Returns(cartResponse);
            // Act
            var result = await cartController.GetCart(CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(cartResponse));
        }
        [Test]
        public async Task AddBookToCart_ValidRequest_ReturnsBookListingResponse()
        {
            // Arrange
            var request = new AddCartBookToCartRequest { BookId = 1, BookAmount = 2 };
            var cart = new Cart { Id = "cart-1", UserId = "test-user" };
            var cartBook = new CartBook { Id = "book-1", BookAmount = 2, BookId = 1 };
            var bookListingResponse = new BookListingResponse { Id = "book-1", BookAmount = 2, BookId = 1, Book = new BookResponse() };

            mockCartService.Setup(c => c.GetCartByUserIdAsync(It.IsAny<string>(), false, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(cart);
            mockMapper.Setup(m => m.Map<CartBook>(request)).Returns(cartBook);
            mockCartService.Setup(c => c.AddCartBookAsync(cart, cartBook, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(cartBook);
            mockMapper.Setup(m => m.Map<BookListingResponse>(cartBook)).Returns(bookListingResponse);
            // Act
            var result = await cartController.AddBookToCart(request, CancellationToken.None);
            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(bookListingResponse));
        }
        [Test]
        public async Task UpdateCartBookInCart_ValidRequest_ReturnsUpdatedBookListingResponse()
        {
            // Arrange
            var request = new UpdateCartBookRequest { Id = "book-1", BookAmount = 3 };
            var cart = new Cart { Id = "cart-1", UserId = "test-user" };
            var cartBook = new CartBook { Id = "book-1", BookAmount = 2, BookId = 1 };
            var updatedCartBook = new CartBook { Id = "book-1", BookAmount = 3, BookId = 1 };
            var bookListingResponse = new BookListingResponse { Id = "book-1", BookAmount = 3, BookId = 1, Book = new BookResponse() };
            mockCartService.Setup(c => c.GetCartByUserIdAsync(It.IsAny<string>(), false, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(cart);
            mockCartService.Setup(c => c.CheckBookCartAsync(cart, request.Id, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(true);
            mockMapper.Setup(m => m.Map<CartBook>(request)).Returns(cartBook);
            mockCartService.Setup(c => c.UpdateCartBookAsync(cart, cartBook, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(updatedCartBook);
            mockMapper.Setup(m => m.Map<BookListingResponse>(updatedCartBook)).Returns(bookListingResponse);
            // Act
            var result = await cartController.UpdateCartBookInCart(request, CancellationToken.None);
            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(bookListingResponse));
        }
        [Test]
        public async Task UpdateCartBookInCart_CartBookNotInCart_ReturnsBadRequest()
        {
            // Arrange
            var request = new UpdateCartBookRequest { Id = "book-1", BookAmount = 3 };
            var cart = new Cart { Id = "cart-1", UserId = "test-user" };
            var cartBook = new CartBook { Id = "book-1", BookAmount = 2, BookId = 1 };

            mockCartService.Setup(c => c.GetCartByUserIdAsync(It.IsAny<string>(), false, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(cart);
            mockCartService.Setup(c => c.CheckBookCartAsync(cart, request.Id, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(false);
            mockMapper.Setup(m => m.Map<CartBook>(request)).Returns(cartBook);
            // Act
            var result = await cartController.UpdateCartBookInCart(request, CancellationToken.None);
            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo("Cart book is not found in the cart!"));
        }
        [Test]
        public async Task DeleteCartBookFromCart_ValidRequest_ReturnsOk()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1", UserId = "test-user" };
            var cartBookId = "book-1";
            mockCartService.Setup(c => c.GetCartByUserIdAsync(It.IsAny<string>(), false, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(cart);
            mockCartService.Setup(c => c.CheckBookCartAsync(cart, cartBookId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(true);
            // Act
            var result = await cartController.DeleteCartBookFromCart(cartBookId, CancellationToken.None) as OkResult;
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }
        [Test]
        public async Task DeleteCartBookFromCart_CartBookNotInCart_ReturnsBadRequest()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1", UserId = "test-user" };
            var cartBookId = "book-1";
            mockCartService.Setup(c => c.GetCartByUserIdAsync(It.IsAny<string>(), false, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(cart);
            mockCartService.Setup(c => c.CheckBookCartAsync(cart, cartBookId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(false);
            // Act
            var result = await cartController.DeleteCartBookFromCart(cartBookId, CancellationToken.None) as BadRequestObjectResult;
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo("Cart book is not found in the cart!"));
        }
        [Test]
        public async Task ClearCart_ValidRequest_ReturnsCartResponse()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1", CartBooks = new List<CartBook> { new CartBook { BookId = 1, BookAmount = 2 } } };
            var cartResponse = new CartResponse
            {
                CartBooks = new List<BookListingResponse>
                {
                    new BookListingResponse { Id = "book-1", BookAmount = 2, BookId = 1, Book = new BookResponse() }
                }
            };
            mockCartService.Setup(c => c.GetCartByUserIdAsync(It.IsAny<string>(), false, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(cart);
            mockCartService.Setup(c => c.ClearCartAsync(cart, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(cart);
            mockMapper.Setup(m => m.Map<CartResponse>(cart)).Returns(cartResponse);
            // Act
            var result = await cartController.ClearCart(CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(cartResponse));
        }
    }
}