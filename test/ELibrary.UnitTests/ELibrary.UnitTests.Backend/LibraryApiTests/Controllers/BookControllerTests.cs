using AutoMapper;
using Caching.Helpers;
using Caching.Services;
using LibraryApi.Domain.Dto.Book;
using LibraryApi.Domain.Dtos;
using LibraryApi.Domain.Dtos.Book;
using LibraryApi.Services;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.SharedRequests;
using LibraryShopEntities.Domain.Entities.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace LibraryApi.Controllers.Tests
{
    [TestFixture]
    internal class BookControllerTests
    {
        private Mock<ILibraryEntityService<Book>> mockEntityService;
        private Mock<ICacheService> mockCacheService;
        private Mock<ICachingHelper> mockCachingHelper;
        private Mock<IMapper> mockMapper;
        private Mock<IBookService> mockBookService;
        private BookController controller;

        [SetUp]
        public void Setup()
        {
            mockEntityService = new Mock<ILibraryEntityService<Book>>();
            mockCacheService = new Mock<ICacheService>();

            mockCacheService.Setup(x => x.Get<object>(It.IsAny<string>())).Returns(null);

            mockCachingHelper = new Mock<ICachingHelper>();
            mockMapper = new Mock<IMapper>();
            mockBookService = new Mock<IBookService>();
            controller = new BookController(mockEntityService.Object, mockCacheService.Object, mockCachingHelper.Object, mockMapper.Object, mockBookService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
            }, "mock"));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Test]
        public async Task GetById_ExistingId_ReturnsOk()
        {
            // Arrange
            var bookId = 1;
            var book = new Book { Id = bookId, Name = "Dune" };
            var response = new BookResponse { Id = bookId, Name = "Dune" };
            mockEntityService.Setup(s => s.GetByIdAsync(bookId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);
            mockMapper.Setup(m => m.Map<BookResponse>(book))
                .Returns(response);
            // Act
            var result = await controller.GetById(bookId, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.Value, Is.EqualTo(response));
        }
        [Test]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var bookId = 1;
            mockEntityService.Setup(s => s.GetByIdAsync(bookId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Book)null);
            // Act
            var result = await controller.GetById(bookId, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
        [Test]
        public async Task GetByIds_ValidRequest_ReturnsOkWithItems()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, Name = "Dune" },
                new Book { Id = 2, Name = "1984" }
            };
            var request = new GetByIdsRequest
            {
                Ids = new List<int> { 1, 2, 3 }
            };
            mockEntityService.Setup(s => s.GetByIdsAsync(request.Ids, It.IsAny<CancellationToken>()))
                .ReturnsAsync(books);
            mockMapper.Setup(m => m.Map<BookResponse>(It.IsAny<Book>()))
                .Returns((Book b) => new BookResponse { Id = b.Id, Name = b.Name });
            // Act
            var result = await controller.GetByIds(request, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That((okResult.Value as IEnumerable<BookResponse>).Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task RaisePopularity_ValidIds_ReturnsOk()
        {
            // Arrange
            var bookIds = new List<int> { 1, 2, 3 };
            var request = new RaiseBookPopularityRequest { Ids = bookIds };
            mockBookService.Setup(s => s.RaisePopularityAsync(bookIds, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await controller.RaisePopularity(request, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            mockBookService.Verify(s => s.RaisePopularityAsync(bookIds, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateStockAmount_ValidRequests_ReturnsOk()
        {
            // Arrange
            var requests = new List<UpdateBookStockAmountRequest>
        {
            new UpdateBookStockAmountRequest { BookId = 1, ChangeAmount = 5 },
            new UpdateBookStockAmountRequest { BookId = 2, ChangeAmount = -2 }
        };
            var requestDict = requests.ToDictionary(r => r.BookId, r => r.ChangeAmount);

            mockBookService.Setup(s => s.ChangeBookStockAmount(requestDict, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await controller.UpdateStockAmount(requests, CancellationToken.None);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
            mockBookService.Verify(s => s.ChangeBookStockAmount(requestDict, It.IsAny<CancellationToken>()), Times.Once);
        }
        [Test]
        public async Task GetPaginated_ValidRequest_ReturnsOkWithPaginatedResults()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, Name = "Dune" },
                new Book { Id = 2, Name = "1984" }
            };
            var request = new BookFilterRequest { PageNumber = 1, PageSize = 2 };
            mockEntityService.Setup(s => s.GetPaginatedAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(books);
            mockMapper.Setup(m => m.Map<BookResponse>(It.IsAny<Book>()))
                .Returns((Book b) => new BookResponse { Id = b.Id, Name = b.Name });
            // Act
            var result = await controller.GetPaginated(request, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.Greater((okResult.Value as IEnumerable<BookResponse>).Count(), 1);
        }
        [Test]
        public async Task GetItemTotalAmount_ReturnsAmount()
        {
            // Arrange
            mockEntityService.Setup(s => s.GetItemTotalAmountAsync(It.IsAny<LibraryFilterRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(10);
            // Act
            var result = await controller.GetItemTotalAmount(new BookFilterRequest() { ContainsName = "" }, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.That(okResult.Value, Is.EqualTo(10));
        }
        [Test]
        public async Task Create_ValidRequest_ReturnsCreatedResponse()
        {
            // Arrange
            var createRequest = new CreateBookRequest { Name = "Dune", PublicationDate = new DateTime(1965, 8, 1), AuthorId = 1, GenreId = 1 };
            var book = new Book { Id = 1, Name = "Dune", PublicationDate = new DateTime(1965, 8, 1), AuthorId = 1, GenreId = 1 };
            var createResponse = new BookResponse { Id = 1, Name = "Dune", PublicationDate = new DateTime(1965, 8, 1) };
            mockMapper.Setup(m => m.Map<Book>(createRequest)).Returns(book);
            mockEntityService.Setup(s => s.CreateAsync(book, It.IsAny<CancellationToken>())).ReturnsAsync(book);
            mockMapper.Setup(m => m.Map<BookResponse>(book)).Returns(createResponse);
            // Act
            var result = await controller.Create(createRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<CreatedResult>(result.Result);
            var createdResult = result.Result as CreatedResult;
            Assert.IsNotNull(createdResult);
            Assert.That(createdResult.Value, Is.EqualTo(createResponse));
        }
        [Test]
        public async Task Update_ValidRequest_ReturnsOk()
        {
            // Arrange
            var updateRequest = new UpdateBookRequest { Id = 1, Name = "Dune", PublicationDate = new DateTime(1965, 8, 1), AuthorId = 1, GenreId = 1 };
            var book = new Book { Id = 1, Name = "Dune", PublicationDate = new DateTime(1965, 8, 1), AuthorId = 1, GenreId = 1 };
            var updatedResponse = new BookResponse { Id = 1, Name = "Dune", PublicationDate = new DateTime(1965, 8, 1) };
            mockMapper.Setup(m => m.Map<Book>(updateRequest)).Returns(book);
            mockEntityService.Setup(s => s.UpdateAsync(book, It.IsAny<CancellationToken>())).ReturnsAsync(book);
            mockMapper.Setup(m => m.Map<BookResponse>(book)).Returns(updatedResponse);
            // Act
            var result = await controller.Update(updateRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var updatedResult = result.Result as OkObjectResult;
            Assert.IsNotNull(updatedResult);
            Assert.That(updatedResult.Value, Is.EqualTo(updatedResponse));
        }
        [Test]
        public async Task DeleteById_ExistingId_ReturnsOk()
        {
            // Arrange
            var bookId = 1;
            mockEntityService.Setup(s => s.DeleteByIdAsync(bookId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            // Act
            var result = await controller.DeleteById(bookId, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}