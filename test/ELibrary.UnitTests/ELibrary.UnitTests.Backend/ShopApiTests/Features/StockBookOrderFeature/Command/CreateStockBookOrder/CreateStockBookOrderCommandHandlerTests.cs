using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Moq;
using ShopApi.Features.StockBookOrderFeature.Dtos;
using ShopApi.Features.StockBookOrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.StockBookOrderFeature.Command.CreateStockBookOrder.Tests
{
    [TestFixture]
    internal class CreateStockBookOrderCommandHandlerTests
    {
        private Mock<IStockBookOrderService> stockBookOrderServiceMock;
        private Mock<ILibraryService> libraryServiceMock;
        private Mock<IMapper> mapperMock;
        private CreateStockBookOrderCommandHandler handler;
        private CancellationToken cancellationToken;

        [SetUp]
        public void SetUp()
        {
            stockBookOrderServiceMock = new Mock<IStockBookOrderService>();
            libraryServiceMock = new Mock<ILibraryService>();
            mapperMock = new Mock<IMapper>();
            handler = new CreateStockBookOrderCommandHandler(
                stockBookOrderServiceMock.Object,
                libraryServiceMock.Object,
                mapperMock.Object
            );
            cancellationToken = new CancellationToken();
        }
        [Test]
        public async Task Handle_ValidRequest_ReturnsStockBookOrderResponse()
        {
            // Arrange
            var createRequest = new CreateStockBookOrderRequest
            {
                ClientId = "Client1",
                StockBookChanges = new List<StockBookChangeRequest>
                {
                    new StockBookChangeRequest { BookId = 1, ChangeAmount = 5 },
                    new StockBookChangeRequest { BookId = 2, ChangeAmount = -3 }
                }
            };
            var command = new CreateStockBookOrderCommand(createRequest);
            var orderEntity = new StockBookOrder { Id = 1 };
            var orderResponse = new StockBookOrderResponse { Id = 1, StockBookChanges = new List<StockBookChangeResponse>() };
            mapperMock.Setup(m => m.Map<StockBookOrder>(createRequest)).Returns(orderEntity);
            mapperMock.Setup(m => m.Map<StockBookOrderResponse>(orderEntity)).Returns(orderResponse);
            stockBookOrderServiceMock.Setup(s => s.AddStockBookOrderAsync(orderEntity, cancellationToken))
                .ReturnsAsync(orderEntity);
            var bookResponses = new List<BookResponse>
            {
                new BookResponse { Id = 1, Name = "Book1" },
                new BookResponse { Id = 2, Name = "Book2" }
            };
            libraryServiceMock.Setup(s => s.GetByIdsAsync<BookResponse>(
                    It.Is<List<int>>(ids => ids.SequenceEqual(new List<int> { 1, 2 })),
                    It.IsAny<string>(),
                    cancellationToken))
                .ReturnsAsync(bookResponses);
            // Act
            var result = await handler.Handle(command, cancellationToken);
            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(orderResponse.Id));
            Assert.That(result.StockBookChanges.Count, Is.EqualTo(orderResponse.StockBookChanges.Count));
        }
        [Test]
        public void Handle_BookIdsNotFound_ThrowsInvalidDataException()
        {
            // Arrange
            var createRequest = new CreateStockBookOrderRequest
            {
                ClientId = "Client1",
                StockBookChanges = new List<StockBookChangeRequest>
                {
                    new StockBookChangeRequest { BookId = 1, ChangeAmount = 5 },
                    new StockBookChangeRequest { BookId = 3, ChangeAmount = -2 }
                }
            };
            var command = new CreateStockBookOrderCommand(createRequest);
            libraryServiceMock.Setup(s => s.GetByIdsAsync<BookResponse>(
                    It.IsAny<List<int>>(),
                    It.IsAny<string>(),
                    cancellationToken))
                .ReturnsAsync(new List<BookResponse> { new BookResponse { Id = 1, Name = "Book1" } });
            // Act & Assert
            Assert.ThrowsAsync<InvalidDataException>(() => handler.Handle(command, cancellationToken));
        }
    }
}