using LibraryApi.Domain.Dto.Author;
using LibraryApi.Domain.Dtos;
using LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.IntegrationTests.Controllers.AuthorController
{
    internal class GetPaginatedAuthorControllerTests : GetPaginatedBaseLibraryControllerTests<Author, CreateAuthorRequest, AuthorResponse, LibraryFilterRequest>
    {
        protected override string ControllerEndpoint => AuthorControllerTestHelper.ControllerEndpoint;

        protected override async Task<List<Author?>> CreateSamplesAsync()
        {
            return await AuthorControllerTestHelper.CreateSamplesAsync(CreateSampleEntityAsync);
        }

        protected override LibraryFilterRequest GetFilter()
        {
            return new LibraryFilterRequest();
        }
    }
}
