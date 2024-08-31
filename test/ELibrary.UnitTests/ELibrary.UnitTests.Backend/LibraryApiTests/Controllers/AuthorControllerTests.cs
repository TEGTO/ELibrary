using AutoMapper;
using LibraryApi.Domain.Dto;
using LibraryApi.Domain.Dto.Author;
using LibraryApi.Domain.Entities;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryApi.Controllers
{
    [TestFixture]
    internal class AuthorControllerTests
    {
        private Mock<ILibraryEntityService<Author>> mockEntityService;
        private Mock<IMapper> mockMapper;
        private AuthorController controller;

        [SetUp]
        public void Setup()
        {
            mockEntityService = new Mock<ILibraryEntityService<Author>>();
            mockMapper = new Mock<IMapper>();
            controller = new AuthorController(mockEntityService.Object, mockMapper.Object);
        }

        [Test]
        public async Task GetById_ExistingId_ReturnsOk()
        {
            // Arrange
            var authorId = 1;
            var author = new Author { Id = authorId, Name = "John", LastName = "Doe" };
            var response = new AuthorResponse { Id = authorId, Name = "John", LastName = "Doe" };
            mockEntityService.Setup(s => s.GetByIdAsync(authorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(author);
            mockMapper.Setup(m => m.Map<AuthorResponse>(author))
                .Returns(response);
            // Act
            var result = await controller.GetById(authorId, CancellationToken.None);
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
            var authorId = 1;
            mockEntityService.Setup(s => s.GetByIdAsync(authorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Author)null);
            // Act
            var result = await controller.GetById(authorId, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
        [Test]
        public async Task GetPaginated_ValidRequest_ReturnsOkWithPaginatedResults()
        {
            // Arrange
            var authors = new List<Author>
            {
                new Author { Id = 1, Name = "John", LastName = "Doe" },
                new Author { Id = 2, Name = "Jane", LastName = "Doe" }
            };
            var request = new PaginatedRequest { PageNumber = 1, PageSize = 2 };
            var responses = authors.Select(a => new AuthorResponse { Id = a.Id, Name = a.Name, LastName = a.LastName }).ToList();
            mockEntityService.Setup(s => s.GetPaginatedAsync(request.PageNumber, request.PageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(authors);
            mockMapper.Setup(m => m.Map<AuthorResponse>(It.IsAny<Author>()))
                .Returns((Author a) => new AuthorResponse { Id = a.Id, Name = a.Name, LastName = a.LastName });
            // Act
            var result = await controller.GetPaginated(request, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.Greater((okResult.Value as IEnumerable<AuthorResponse>).Count(), 1);
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
            var createRequest = new CreateAuthorRequest { Name = "John", LastName = "Doe" };
            var author = new Author { Id = 1, Name = "John", LastName = "Doe" };
            var createResponse = new AuthorResponse { Id = 1, Name = "John", LastName = "Doe" };
            mockMapper.Setup(m => m.Map<Author>(createRequest)).Returns(author);
            mockEntityService.Setup(s => s.CreateAsync(author, It.IsAny<CancellationToken>())).ReturnsAsync(author);
            mockMapper.Setup(m => m.Map<AuthorResponse>(author)).Returns(createResponse);
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
            var updateRequest = new UpdateAuthorRequest { Id = 1, Name = "John", LastName = "Doe" };
            var author = new Author { Id = 1, Name = "John", LastName = "Doe" };
            mockMapper.Setup(m => m.Map<Author>(updateRequest)).Returns(author);
            mockEntityService.Setup(s => s.UpdateAsync(author, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            // Act
            var result = await controller.Update(updateRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task DeleteById_ExistingId_ReturnsOk()
        {
            // Arrange
            var authorId = 1;
            mockEntityService.Setup(s => s.DeleteByIdAsync(authorId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            // Act
            var result = await controller.DeleteById(authorId, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}