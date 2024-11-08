using LibraryApi.Domain.Dto.Author;
using LibraryApi.Domain.Dto.Book;
using LibraryApi.Domain.Dto.Genre;
using LibraryApi.Domain.Dto.Publisher;
using LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;

namespace LibraryApi.IntegrationTests.Controllers.BookController
{
    internal class GetAmountBookControllerTests : GetAmountBaseLibraryControllerTests<Book, CreateBookRequest, BookResponse, LibraryFilterRequest>
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

        protected override LibraryFilterRequest GetFilter()
        {
            return new LibraryFilterRequest();
        }
    }
}