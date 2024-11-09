using LibraryApi.Domain.Dto.Genre;
using LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.IntegrationTests.Controllers.GenreController
{
    internal class DeleteGenreControllerTests : DeleteLibraryEntityControllerTests<Genre, CreateGenreRequest, GenreResponse>
    {
        protected override string ControllerEndpoint => GenreControllerTestHelper.ControllerEndpoint;

        protected override async Task<List<Genre?>> CreateSamplesAsync()
        {
            return await GenreControllerTestHelper.CreateSamplesAsync(CreateSampleEntityAsync);
        }
    }
}
