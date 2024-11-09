using LibraryApi.Domain.Dto.Genre;
using LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.IntegrationTests.Controllers.GenreController
{
    internal class UpdateGenreControllerTests : UpdateBaseLibraryEntityControllerTests<Genre, CreateGenreRequest, GenreResponse, UpdateGenreRequest>
    {
        protected override string ControllerEndpoint => GenreControllerTestHelper.ControllerEndpoint;

        protected override async Task<List<Genre?>> CreateSamplesAsync()
        {
            return await GenreControllerTestHelper.CreateSamplesAsync(CreateSampleEntityAsync);
        }

        protected override UpdateGenreRequest GetUpdateRequest()
        {
            return new UpdateGenreRequest()
            {
                Id = 1,
                Name = "NewGenreName",
            };
        }

        protected override UpdateGenreRequest GetInvalidUpdateRequest()
        {
            return new UpdateGenreRequest();
        }
    }
}
