﻿using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.CartFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.CartFeature.Command.GetCart.Tests
{
    [TestFixture]
    internal class GetCartQueryHandlerTests
    {
        private Mock<ICartService> mockCartService;
        private Mock<IGetLibraryItemsService> mockGetItemsService;
        private Mock<IMapper> mockMapper;
        private GetCartQueryHandler handler;

        [SetUp]
        public void SetUp()
        {
            mockCartService = new Mock<ICartService>();
            mockGetItemsService = new Mock<IGetLibraryItemsService>();
            mockMapper = new Mock<IMapper>();
            handler = new GetCartQueryHandler(mockCartService.Object, mockGetItemsService.Object, mockMapper.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnMappedNewCart_WhenCartDoesNotExist()
        {
            // Arrange
            var query = new GetCartQuery("user123");
            var cancellationToken = CancellationToken.None;
            var newCart = new Cart { Id = "newCartId" };
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), true, cancellationToken))
                .ReturnsAsync((Cart)null);
            mockCartService.Setup(cs => cs.CreateCartAsync(It.IsAny<string>(), cancellationToken))
                .ReturnsAsync(newCart);
            var expectedCartResponse = new CartResponse();
            mockMapper.Setup(m => m.Map<CartResponse>(newCart)).Returns(expectedCartResponse);
            // Act
            var result = await handler.Handle(query, cancellationToken);
            // Assert
            mockCartService.Verify(cs => cs.CreateCartAsync(query.UserId, cancellationToken), Times.Once);
            Assert.That(result, Is.EqualTo(expectedCartResponse));
        }
        [Test]
        public async Task Handle_ShouldReturnMappedExistingCartWithBooks_WhenCartExists()
        {
            // Arrange
            var query = new GetCartQuery("user123");
            var cancellationToken = CancellationToken.None;
            var existingCart = new Cart
            {
                Id = "existingCartId",
                Books = new List<CartBook> { new CartBook { BookId = 1 } }
            };
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), true, cancellationToken))
                .ReturnsAsync(existingCart);
            var bookResponses = new List<BookResponse> { new BookResponse { Id = 1, Name = "Book1" } };
            mockGetItemsService.Setup(s => s.GetByIdsAsync<BookResponse>(
                It.IsAny<List<int>>(),
                It.IsAny<string>(),
                cancellationToken)
            ).ReturnsAsync(bookResponses);
            var mappedCartResponse = new CartResponse
            {
                Books = new List<BookListingResponse>
                {
                    new BookListingResponse { BookId = 1, Book = bookResponses.First() }
                }
            };
            mockMapper.Setup(m => m.Map<CartResponse>(existingCart)).Returns(mappedCartResponse);
            // Act
            var result = await handler.Handle(query, cancellationToken);
            // Assert
            Assert.NotNull(result);
            Assert.That(result.Books.Count, Is.EqualTo(1));
            Assert.That(result.Books.First().Book.Name, Is.EqualTo("Book1"));
        }
        [Test]
        public async Task Handle_ShouldMapBookResponseCorrectly_WhenCartBooksExist()
        {
            // Arrange
            var query = new GetCartQuery("user123");
            var cancellationToken = CancellationToken.None;
            var existingCart = new Cart
            {
                Id = "cartId",
                Books = new List<CartBook> { new CartBook { BookId = 2 } }
            };
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), true, cancellationToken))
                .ReturnsAsync(existingCart);
            var bookResponses = new List<BookResponse> { new BookResponse { Id = 2, Name = "Mapped Book" } };
            mockGetItemsService.Setup(s => s.GetByIdsAsync<BookResponse>(
                It.IsAny<List<int>>(),
                It.IsAny<string>(),
                cancellationToken)
            ).ReturnsAsync(bookResponses);
            var cartResponse = new CartResponse
            {
                Books = new List<BookListingResponse> { new BookListingResponse { BookId = 2 } }
            };
            mockMapper.Setup(m => m.Map<CartResponse>(existingCart)).Returns(cartResponse);
            // Act
            var result = await handler.Handle(query, cancellationToken);
            // Assert
            Assert.That(result.Books.First().BookId, Is.EqualTo(2));
            Assert.That(result.Books.First().Book.Name, Is.EqualTo("Mapped Book"));
        }
    }
}