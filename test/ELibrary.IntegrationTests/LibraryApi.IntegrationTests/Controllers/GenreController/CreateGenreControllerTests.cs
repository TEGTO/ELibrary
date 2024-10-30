using LibraryApi.Domain.Dto.Genre;
using LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.IntegrationTests.Controllers.GenreController
{
    internal class CreateGenreControllerTests : CreateBaseLibraryEntityControllerTests<Genre, CreateGenreRequest, GenreResponse>
    {
        protected override string ControllerEndpoint => GenreControllerTestHelper.ControllerEndpoint;

        protected override async Task<List<Genre?>> CreateSamplesAsync()
        {
            return await GenreControllerTestHelper.CreateSamplesAsync(CreateSampleEntityAsync);
        }

        protected override async ValueTask<CreateGenreRequest> GetCreateRequestAsync()
        {
            return new CreateGenreRequest()
            {
                Name = "Genre",
            };
        }

        protected override async ValueTask<CreateGenreRequest> GetInvalidCreateRequestAsync()
        {
            return new CreateGenreRequest();
        }
    }
}
