using LibraryApi.Domain.Dto.Author;
using LibraryApi.Domain.Dto.Book;
using LibraryApi.Domain.Dto.Genre;
using LibraryApi.Domain.Dto.Publisher;
using LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.IntegrationTests.Controllers.BookController
{
    internal class GetByIdBookControllerTests : GetByIdBaseLibraryEntityControllerTests<Book, CreateBookRequest, BookResponse>
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
    }
}
