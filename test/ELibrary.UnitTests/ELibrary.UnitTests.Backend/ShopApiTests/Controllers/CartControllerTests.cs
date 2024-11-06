using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.Services;
using ShopApi.Features.CartFeature.Command.AddBookToCart;
using ShopApi.Features.CartFeature.Command.DeleteBooksFromCart;
using ShopApi.Features.CartFeature.Command.GetCart;
using ShopApi.Features.CartFeature.Command.GetInCartAmount;
using ShopApi.Features.CartFeature.Command.UpdateCartBookInCart;
using ShopApi.Features.CartFeature.Dtos;
using System.Security.Claims;

namespace ShopApi.Controllers.Tests
{
    [TestFixture]
    internal class CartControllerTests
    {
        private Mock<IMediator> mockMediator;
        private Mock<ICacheService> mockCacheService;
        private CartController cartController;

        [SetUp]
        public void SetUp()
        {
            mockMediator = new Mock<IMediator>();
            mockCacheService = new Mock<ICacheService>();
            cartController = new CartController(mockMediator.Object, mockCacheService.Object);

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
            var cartResponse = new CartResponse
            {
                Books = new List<CartBookResponse>
                {
                    new CartBookResponse { Id = "book-1", BookAmount = 2, BookId = 1, Book = new BookResponse() }
                }
            };
            mockMediator.Setup(m => m.Send(It.IsAny<GetCartQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(cartResponse);
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
            var expectedAmount = 5;
            mockMediator.Setup(m => m.Send(It.IsAny<GetInCartAmountQuery>(), It.IsAny<CancellationToken>()))
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
        public async Task AddBookToCart_ValidRequest_ReturnsBookListingResponse()
        {
            // Arrange
            var request = new AddBookToCartRequest { BookId = 1, BookAmount = 2 };
            var bookListingResponse = new CartBookResponse { Id = "book-1", BookAmount = 2, BookId = 1, Book = new BookResponse() };
            mockMediator.Setup(m => m.Send(It.IsAny<AddBookToCartCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(bookListingResponse);
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
            var updatedBookListingResponse = new CartBookResponse { Id = "book-1", BookAmount = 3, BookId = 1, Book = new BookResponse() };
            mockMediator.Setup(m => m.Send(It.IsAny<UpdateCartBookInCartCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(updatedBookListingResponse);
            // Act
            var result = await cartController.UpdateCartBookInCart(request, CancellationToken.None);
            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(updatedBookListingResponse));
        }
        [Test]
        public async Task DeleteBooksFromCart_ValidRequests_ReturnsUpdatedCartResponse()
        {
            // Arrange
            var requests = new[]
            {
                new DeleteCartBookFromCartRequest { Id = 1 },
                new DeleteCartBookFromCartRequest { Id = 2 }
            };
            var cartResponse = new CartResponse();

            mockMediator.Setup(m => m.Send(It.IsAny<DeleteBooksFromCartCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(cartResponse);
            // Act
            var result = await cartController.DeleteBooksFromCart(requests, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(cartResponse));
        }
    }
}