using AutoMapper;
using LibraryShopEntities.Domain.Dto;
using LibraryShopEntities.Domain.Dto.Book;
using LibraryShopEntities.Domain.Dto.Library.Book;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryShopEntities.Controllers
{
    [TestFixture]
    internal class BookControllerTests
    {
        private Mock<ILibraryEntityService<Book>> mockEntityService;
        private Mock<IMapper> mockMapper;
        private BookController controller;

        [SetUp]
        public void Setup()
        {
            mockEntityService = new Mock<ILibraryEntityService<Book>>();
            mockMapper = new Mock<IMapper>();
            controller = new BookController(mockEntityService.Object, mockMapper.Object);
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
        public async Task GetPaginated_ValidRequest_ReturnsOkWithPaginatedResults()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { Id = 1, Name = "Dune" },
                new Book { Id = 2, Name = "1984" }
            };
            var request = new PaginationRequest { PageNumber = 1, PageSize = 2 };
            var responses = books.Select(b => new BookResponse { Id = b.Id, Name = b.Name }).ToList();
            mockEntityService.Setup(s => s.GetPaginatedAsync(request.PageNumber, request.PageSize, It.IsAny<CancellationToken>()))
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
            mockEntityService.Setup(s => s.GetItemTotalAmountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(10);
            // Act
            var result = await controller.GetItemTotalAmount(CancellationToken.None);
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