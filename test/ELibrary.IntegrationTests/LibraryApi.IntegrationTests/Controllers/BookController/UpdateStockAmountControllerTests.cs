using LibraryApi.Domain.Dto.Author;
using LibraryApi.Domain.Dto.Book;
using LibraryApi.Domain.Dto.Genre;
using LibraryApi.Domain.Dto.Publisher;
using LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.SharedRequests;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;
using System.Net;
using System.Text;
using System.Text.Json;

namespace LibraryApi.IntegrationTests.Controllers.BookController
{
    internal class UpdateStockAmountControllerTests : BaseLibraryEntityControllerTest<Book, CreateBookRequest, BookResponse>
    {
        protected List<Book> list;

        protected override string ControllerEndpoint => BookControllerTestHelper.ControllerEndpoint;

        [OneTimeSetUp]
        public async Task CreateSamples()
        {
            list = await CreateSamplesAsync();
        }

        protected override async Task<List<Book?>> CreateSamplesAsync()
        {
            return await BookControllerTestHelper.CreateSamplesAsync(
               CreateSampleEntityAsync,
               CreateSampleEntityAsync<Author, CreateAuthorRequest, AuthorResponse>,
               CreateSampleEntityAsync<Genre, CreateGenreRequest, GenreResponse>,
               CreateSampleEntityAsync<Publisher, CreatePublisherRequest, PublisherResponse>
           );
        }

        [Test]
        public async Task UpdateStockAmount_ValidRequest_ReturnsOKAndUpdatesStockAmount()
        {
            // Arrange
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/stockamount");
            var raiseRequest = new List<UpdateBookStockAmountRequest> { new UpdateBookStockAmountRequest { BookId = list[^1].Id, ChangeAmount = 2 } };
            request.Content = new StringContent(
                JsonSerializer.Serialize(raiseRequest),
                Encoding.UTF8,
                "application/json"
            );
            // Act 
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var books = await GetPaginatedAsync();

            Assert.NotNull(books);
            Assert.That(books.Count, Is.EqualTo(list.Count));
            Assert.That(books.Find(x => x.Id == list[^1].Id).StockAmount, Is.EqualTo(2));
        }
        [Test]
        public async Task UpdateStockAmount_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/stockamount");
            var raiseRequest = new List<UpdateBookStockAmountRequest> { new UpdateBookStockAmountRequest { BookId = -1 } };
            request.Content = new StringContent(
                JsonSerializer.Serialize(raiseRequest),
                Encoding.UTF8,
                "application/json"
            );
            // Act 
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task UpdateStockAmount_ValidRequestWithNotExistingId_ReturnsOkAndDoesntUpdateStockAmount()
        {
            // Arrange
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/stockamount");
            var raiseRequest = new List<UpdateBookStockAmountRequest> { new UpdateBookStockAmountRequest { BookId = 100, ChangeAmount = -2 } };
            request.Content = new StringContent(
                JsonSerializer.Serialize(raiseRequest),
                Encoding.UTF8,
                "application/json"
            );
            // Act 
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var books = await GetPaginatedAsync();

            Assert.NotNull(books);
            Assert.That(books.Count, Is.EqualTo(list.Count));
            Assert.That(books.FirstOrDefault(x => x.Id == 100), Is.Null);
        }

        private async Task<List<BookResponse>> GetPaginatedAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/pagination");
            var filter = new LibraryFilterRequest();
            filter.PageNumber = 1;
            filter.PageSize = 10;
            request.Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");
            // Act 
            var response = await client.SendAsync(request);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var content = await response.Content.ReadAsStringAsync();
            var responseEntities = JsonSerializer.Deserialize<List<BookResponse>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return responseEntities;
        }
    }
}
