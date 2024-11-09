using LibraryApi.Domain.Dto.Genre;
using LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;

namespace LibraryApi.IntegrationTests.Controllers.GenreController
{
    internal class GetPaginatedGenreControllerTests : GetPaginatedBaseLibraryControllerTests<Genre, CreateGenreRequest, GenreResponse, LibraryFilterRequest>
    {
        protected override string ControllerEndpoint => GenreControllerTestHelper.ControllerEndpoint;

        protected override async Task<List<Genre?>> CreateSamplesAsync()
        {
            return await GenreControllerTestHelper.CreateSamplesAsync(CreateSampleEntityAsync);
        }

        protected override LibraryFilterRequest GetFilter()
        {
            return new LibraryFilterRequest();
        }
    }
}
