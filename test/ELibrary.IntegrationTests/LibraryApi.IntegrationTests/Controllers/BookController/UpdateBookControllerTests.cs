using LibraryApi.Domain.Dto.Author;
using LibraryApi.Domain.Dto.Book;
using LibraryApi.Domain.Dto.Genre;
using LibraryApi.Domain.Dto.Publisher;
using LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.IntegrationTests.Controllers.BookController
{
    internal class UpdateBookControllerTests : UpdateBaseLibraryEntityControllerTests<Book, CreateBookRequest, BookResponse, UpdateBookRequest>
    {
        protected override string ControllerEndpoint => BookControllerTestHelper.ControllerEndpoint;

        protected override async Task<List<Book?>> CreateSamplesAsync()
        {
            return await BookControllerTestHelper.CreateSamplesAsync(
              CreateSampleEntityAsync,
              CreateSampleEntityAsync<Author, CreateAuthorRequest, AuthorResponse>,
              CreateSampleEntityAsync<Genre, CreateGenreRequest, GenreResponse>,
              CreateSampleEntityAsync<Publisher, CreatePublisherRequest, PublisherResponse>
          );
        }

        protected override UpdateBookRequest GetUpdateRequest()
        {
            return new UpdateBookRequest()
            {
                Id = 1,
                Name = "NewBookName",
                Price = 100,
                PublicationDate = new DateTime(1949, 6, 8, 0, 0, 0, DateTimeKind.Utc),
                CoverType = CoverType.Hard,
                CoverImgUrl = "smt",
                PageAmount = 100,
                AuthorId = 1,
                GenreId = 1,
                PublisherId = 1,
            };
        }

        protected override UpdateBookRequest GetInvalidUpdateRequest()
        {
            return new UpdateBookRequest();
        }
    }
}
