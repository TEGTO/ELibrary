using AutoMapper;
using LibraryApi.Domain.Dto;
using LibraryApi.Domain.Dto.Book;
using LibraryApi.Domain.Entities;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryApi.Controllers
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
            var book = new Book { Id = bookId, Title = "Dune" };
            var response = new GetBookResponse { Id = bookId, Title = "Dune" };
            mockEntityService.Setup(s => s.GetByIdAsync(bookId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);
            mockMapper.Setup(m => m.Map<GetBookResponse>(book))
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
                new Book { Id = 1, Title = "Dune" },
                new Book { Id = 2, Title = "1984" }
            };
            var request = new PaginatedRequest { PageNumber = 1, PageSize = 2 };
            var responses = books.Select(b => new GetBookResponse { Id = b.Id, Title = b.Title }).ToList();
            mockEntityService.Setup(s => s.GetPaginatedAsync(request.PageNumber, request.PageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(books);
            mockMapper.Setup(m => m.Map<GetBookResponse>(It.IsAny<Book>()))
                .Returns((Book b) => new GetBookResponse { Id = b.Id, Title = b.Title });
            // Act
            var result = await controller.GetPaginated(request, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.Greater((okResult.Value as IEnumerable<GetBookResponse>).Count(), 1);
        }
        [Test]
        public async Task Create_ValidRequest_ReturnsCreatedResponse()
        {
            // Arrange
            var createRequest = new CreateBookRequest { Title = "Dune", PublicationDate = new DateTime(1965, 8, 1), AuthorId = "1", GenreId = "1" };
            var book = new Book { Id = 1, Title = "Dune", PublicationDate = new DateTime(1965, 8, 1), AuthorId = 1, GenreId = 1 };
            var createResponse = new CreateBookResponse { Id = 1, Title = "Dune", PublicationDate = new DateTime(1965, 8, 1), AuthorId = "1", GenreId = "1" };
            mockMapper.Setup(m => m.Map<Book>(createRequest)).Returns(book);
            mockEntityService.Setup(s => s.CreateAsync(book, It.IsAny<CancellationToken>())).ReturnsAsync(book);
            mockMapper.Setup(m => m.Map<CreateBookResponse>(book)).Returns(createResponse);
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
            var updateRequest = new UpdateBookRequest { Id = 1, Title = "Dune", PublicationDate = new DateTime(1965, 8, 1), AuthorId = "1", GenreId = "1" };
            var book = new Book { Id = 1, Title = "Dune", PublicationDate = new DateTime(1965, 8, 1), AuthorId = 1, GenreId = 1 };
            mockMapper.Setup(m => m.Map<Book>(updateRequest)).Returns(book);
            mockEntityService.Setup(s => s.UpdateAsync(book, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            // Act
            var result = await controller.Update(updateRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
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