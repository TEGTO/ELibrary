﻿using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.CartFeature.Dtos;
using ShopApi.Features.CartFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.CartFeature.Command.UpdateCartBookInCart.Tests
{
    [TestFixture]
    internal class UpdateCartBookInCartCommandHandlerTests
    {
        private Mock<ICartService> mockCartService;
        private Mock<IGetLibraryItemsService> mockGetItemsService;
        private Mock<IMapper> mockMapper;
        private UpdateCartBookInCartCommandHandler handler;

        [SetUp]
        public void SetUp()
        {
            mockCartService = new Mock<ICartService>();
            mockGetItemsService = new Mock<IGetLibraryItemsService>();
            mockMapper = new Mock<IMapper>();
            handler = new UpdateCartBookInCartCommandHandler(mockCartService.Object, mockGetItemsService.Object, mockMapper.Object);
        }

        [Test]
        public async Task Handle_ShouldCreateCartAndReturnMappedBookListingResponse_WhenCartDoesNotExist()
        {
            // Arrange
            var command = new UpdateCartBookInCartCommand("user123", new UpdateCartBookRequest { Id = "book1", BookAmount = 2 });
            var cancellationToken = CancellationToken.None;
            var newCart = new Cart { Id = "newCartId" };
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), false, cancellationToken))
                .ReturnsAsync((Cart)null);
            mockCartService.Setup(cs => cs.CreateCartAsync(It.IsAny<string>(), cancellationToken))
                .ReturnsAsync(newCart);
            var cartBook = new CartBook();
            mockMapper.Setup(m => m.Map<CartBook>(command.Request)).Returns(cartBook);
            mockCartService.Setup(cs => cs.CheckBookCartAsync(It.IsAny<Cart>(), It.IsAny<string>(), cancellationToken))
                .ReturnsAsync(true);
            var bookResponse = new BookResponse { Id = 1, Name = "Test Book" };
            mockGetItemsService.Setup(s => s.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), cancellationToken))
             .ReturnsAsync(new List<BookResponse> { bookResponse });
            var updatedCartBook = new CartBook();
            mockCartService.Setup(cs => cs.UpdateCartBookAsync(It.IsAny<Cart>(), It.IsAny<CartBook>(), cancellationToken))
                .ReturnsAsync(updatedCartBook);
            var mappedResponse = new BookListingResponse();
            mockMapper.Setup(m => m.Map<BookListingResponse>(updatedCartBook)).Returns(mappedResponse);
            // Act
            var result = await handler.Handle(command, cancellationToken);
            // Assert
            Assert.That(result, Is.EqualTo(mappedResponse));
        }
        [Test]
        public void Handle_ShouldThrowException_WhenCartBookNotFoundInCart()
        {
            // Arrange
            var command = new UpdateCartBookInCartCommand("user123", new UpdateCartBookRequest { Id = "nonExistentBook", BookAmount = 2 });
            var cancellationToken = CancellationToken.None;
            var cart = new Cart { Id = "existingCartId" };
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), false, cancellationToken))
                .ReturnsAsync(cart);
            var cartBook = new CartBook();
            mockMapper.Setup(m => m.Map<CartBook>(command.Request)).Returns(cartBook);
            mockCartService.Setup(cs => cs.CheckBookCartAsync(It.IsAny<Cart>(), It.IsAny<string>(), cancellationToken))
                .ReturnsAsync(false);
            // Act & Assert
            Assert.ThrowsAsync<InvalidDataException>(async () => await handler.Handle(command, cancellationToken));
        }
        [Test]
        public async Task Handle_ShouldReturnMappedBookListingResponse_WithCorrectBookDetails()
        {
            // Arrange
            var command = new UpdateCartBookInCartCommand("user123", new UpdateCartBookRequest { Id = "book1", BookAmount = 3 });
            var cancellationToken = CancellationToken.None;
            var existingCart = new Cart { Id = "existingCartId" };
            mockCartService.Setup(cs => cs.GetCartByUserIdAsync(It.IsAny<string>(), false, cancellationToken))
                .ReturnsAsync(existingCart);
            var cartBook = new CartBook();
            mockMapper.Setup(m => m.Map<CartBook>(command.Request)).Returns(cartBook);
            mockCartService.Setup(cs => cs.CheckBookCartAsync(It.IsAny<Cart>(), It.IsAny<string>(), cancellationToken))
                .ReturnsAsync(true);
            var bookResponse = new BookResponse { Id = 1, Name = "Test Book" };
            mockGetItemsService.Setup(s => s.GetByIdsAsync<BookResponse>(It.IsAny<List<int>>(), It.IsAny<string>(), cancellationToken))
                .ReturnsAsync(new List<BookResponse> { bookResponse });
            var updatedCartBook = new CartBook();
            mockCartService.Setup(cs => cs.UpdateCartBookAsync(It.IsAny<Cart>(), It.IsAny<CartBook>(), cancellationToken))
                .ReturnsAsync(updatedCartBook);
            var bookListingResponse = new BookListingResponse();
            mockMapper.Setup(m => m.Map<BookListingResponse>(updatedCartBook)).Returns(bookListingResponse);
            // Act
            var result = await handler.Handle(command, cancellationToken);
            // Assert
            Assert.That(result.Book, Is.EqualTo(bookResponse));
            Assert.That(result, Is.EqualTo(bookListingResponse));
        }
    }
}