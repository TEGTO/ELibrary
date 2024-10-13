using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopApi.Features.CartFeature.Dtos;
using ShopApi.Features.CartFeature.Services;
using System.Security.Claims;

namespace ShopApi.Controllers.Tests
{
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
                Books = new List<CartBook>
                {
                    new CartBook { Id = "book-1", BookAmount = 2, BookId = 1, Book = new Book() }
                }
            };
            var cartResponse = new CartResponse
            {
                Books = new List<BookListingResponse>
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
        public async Task GetInCartAmount_CartExists_ReturnsAmount()
        {
            // Arrange
            var cart = new Cart { Id = "cart-1", UserId = "test-user" };
            var expectedAmount = 5;

            mockCartService.Setup(c => c.GetCartByUserIdAsync(It.IsAny<string>(), true, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(cart);
            mockCartService.Setup(c => c.GetInCartAmountAsync(cart, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(expectedAmount);

            // Act
            var result = await cartController.GetInCartAmount(CancellationToken.None);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(expectedAmount));
        }

        [Test]
        public async Task GetInCartAmount_CartNotFound_ReturnsZero()
        {
            // Arrange
            mockCartService.Setup(c => c.GetCartByUserIdAsync(It.IsAny<string>(), true, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((Cart?)null);
            // Act
            var result = await cartController.GetInCartAmount(CancellationToken.None);
            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(0));
        }
        [Test]
        public async Task AddBookToCart_ValidRequest_ReturnsBookListingResponse()
        {
            // Arrange
            var request = new AddBookToCartRequest { BookId = 1, BookAmount = 2 };
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
        public async Task DeleteBooksFromCart_ValidRequests_CallsDeleteBooksFromCartAndReturnsOk()
        {
            // Arrange
            var requests = new[]
            {
                new DeleteCartBookFromCartRequest { Id = 1 },
                new DeleteCartBookFromCartRequest { Id = 2 }
            };

            var books = new[]
            {
                new Book { Id = 1 },
                new Book { Id = 2 }
            };
            var cart = new Cart { Id = "cart-1", Books = new List<CartBook>() };
            var cartResponse = new CartResponse();
            mockMapper.Setup(m => m.Map<Book>(requests[0])).Returns(books[0]);
            mockMapper.Setup(m => m.Map<Book>(requests[1])).Returns(books[1]);
            mockCartService.Setup(s => s.DeleteBooksFromCartAsync(cart, books, It.IsAny<CancellationToken>())).ReturnsAsync(cart);
            mockMapper.Setup(m => m.Map<CartResponse>(It.IsAny<Cart>())).Returns(cartResponse);
            // Act
            var result = await cartController.DeleteBooksFromCart(requests, It.IsAny<CancellationToken>());
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(cartResponse));
            mockMapper.Verify(m => m.Map<Book>(requests[0]), Times.Once);
            mockMapper.Verify(m => m.Map<Book>(requests[1]), Times.Once);
            mockCartService.Verify(s => s.DeleteBooksFromCartAsync(It.IsAny<Cart>(), books, It.IsAny<CancellationToken>()), Times.Once);
            mockMapper.Verify(m => m.Map<CartResponse>(It.IsAny<Cart>()), Times.Once);
        }
    }
}