using LibraryApi.Domain.Dto.Author;
using LibraryApi.Domain.Dto.Book;
using LibraryApi.Domain.Dto.Genre;
using LibraryApi.Domain.Dto.Publisher;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.IntegrationTests.Controllers.BookController
{
    internal static class BookControllerTestHelper
    {
        public static string ControllerEndpoint => "book";

        public static async Task<List<Book?>> CreateSamplesAsync(
            Func<CreateBookRequest, Task<Book?>> createSampleBookAsync,
            Func<CreateAuthorRequest, string, Task<Author?>> createSampleAuthorAsync,
            Func<CreateGenreRequest, string, Task<Genre?>> createSampleGenreAsync,
            Func<CreatePublisherRequest, string, Task<Publisher?>> createSamplePublisherAsync
            )
        {
            await CreateEnvironmentForBookAsync(createSampleAuthorAsync, createSampleGenreAsync, createSamplePublisherAsync);

            var requests = new List<CreateBookRequest>
            {
               new CreateBookRequest {
                   Name = "Book",
                   Price = 100,
                   PublicationDate = new DateTime(1949, 6, 8, 0, 0, 0, DateTimeKind.Utc),
                   CoverType = CoverType.Hard,
                   CoverImgUrl = "smt",
                   PageAmount = 100,
                   AuthorId = 1,
                   GenreId = 1,
                   PublisherId = 1,
               },
                new CreateBookRequest {
                   Name = "Book2",
                   Price = 100,
                   PublicationDate = new DateTime(1949, 6, 8, 0, 0, 0, DateTimeKind.Utc),
                   CoverType = CoverType.Hard,
                   CoverImgUrl = "smt",
                   PageAmount = 100,
                   AuthorId = 1,
                   GenreId = 1,
                   PublisherId = 1,
               },
            };

            var responseSlots = new List<Book?>
            {
                await createSampleBookAsync(requests[0]),
                await createSampleBookAsync(requests[1])
            };

            return responseSlots;
        }

        public static async Task CreateEnvironmentForBookAsync(
           Func<CreateAuthorRequest, string, Task<Author?>> createSampleAuthorAsync,
           Func<CreateGenreRequest, string, Task<Genre?>> createSampleGenreAsync,
           Func<CreatePublisherRequest, string, Task<Publisher?>> createSamplePublisherAsync
           )
        {
            await createSampleAuthorAsync(new CreateAuthorRequest()
            {
                Name = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1949, 6, 8, 0, 0, 0, DateTimeKind.Utc)
            }, "author");

            await createSampleGenreAsync(new CreateGenreRequest()
            {
                Name = "Fantasy"
            }, "genre");

            await createSamplePublisherAsync(new CreatePublisherRequest()
            {
                Name = "Publisher"
            }, "publisher");
        }
    }
}