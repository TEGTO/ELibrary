using AutoMapper;
using LibraryApi.Domain.Dto;
using LibraryApi.Domain.Dto.Genre;
using LibraryApi.Domain.Entities;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryApi.Controllers
{
    [TestFixture]
    internal class GenreControllerTests
    {
        private Mock<ILibraryEntityService<Genre>> mockEntityService;
        private Mock<IMapper> mockMapper;
        private GenreController controller;

        [SetUp]
        public void Setup()
        {
            mockEntityService = new Mock<ILibraryEntityService<Genre>>();
            mockMapper = new Mock<IMapper>();
            controller = new GenreController(mockEntityService.Object, mockMapper.Object);
        }

        [Test]
        public async Task GetById_ExistingId_ReturnsOk()
        {
            // Arrange
            var genreId = 1;
            var genre = new Genre { Id = genreId, Name = "Science Fiction" };
            var response = new GenreResponse { Id = genreId, Name = "Science Fiction" };
            mockEntityService.Setup(s => s.GetByIdAsync(genreId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(genre);
            mockMapper.Setup(m => m.Map<GenreResponse>(genre))
                .Returns(response);
            // Act
            var result = await controller.GetById(genreId, CancellationToken.None);
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
            var genreId = 1;
            mockEntityService.Setup(s => s.GetByIdAsync(genreId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Genre)null);
            // Act
            var result = await controller.GetById(genreId, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
        [Test]
        public async Task GetPaginated_ValidRequest_ReturnsOkWithPaginatedResults()
        {
            // Arrange
            var genres = new List<Genre>
            {
                new Genre { Id = 1, Name = "Science Fiction" },
                new Genre { Id = 2, Name = "Fantasy" }
            };
            var request = new PaginatedRequest { PageNumber = 1, PageSize = 2 };
            var responses = genres.Select(g => new GenreResponse { Id = g.Id, Name = g.Name }).ToList();
            mockEntityService.Setup(s => s.GetPaginatedAsync(request.PageNumber, request.PageSize, It.IsAny<CancellationToken>()))
                .ReturnsAsync(genres);
            mockMapper.Setup(m => m.Map<GenreResponse>(It.IsAny<Genre>()))
                .Returns((Genre g) => new GenreResponse { Id = g.Id, Name = g.Name });
            // Act
            var result = await controller.GetPaginated(request, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.Greater((okResult.Value as IEnumerable<GenreResponse>).Count(), 1);
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
            var createRequest = new CreateGenreRequest { Name = "Mystery" };
            var genre = new Genre { Id = 1, Name = "Mystery" };
            var createResponse = new GenreResponse { Id = 1, Name = "Mystery" };
            mockMapper.Setup(m => m.Map<Genre>(createRequest)).Returns(genre);
            mockEntityService.Setup(s => s.CreateAsync(genre, It.IsAny<CancellationToken>())).ReturnsAsync(genre);
            mockMapper.Setup(m => m.Map<GenreResponse>(genre)).Returns(createResponse);
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
            var updateRequest = new UpdateGenreRequest { Id = 1, Name = "Thriller" };
            var genre = new Genre { Id = 1, Name = "Thriller" };
            mockMapper.Setup(m => m.Map<Genre>(updateRequest)).Returns(genre);
            mockEntityService.Setup(s => s.UpdateAsync(genre, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            // Act
            var result = await controller.Update(updateRequest, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task DeleteById_ExistingId_ReturnsOk()
        {
            // Arrange
            var genreId = 1;
            mockEntityService.Setup(s => s.DeleteByIdAsync(genreId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            // Act
            var result = await controller.DeleteById(genreId, CancellationToken.None);
            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}