using LibraryApi.Domain.Dto.Author;
using LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;

namespace LibraryApi.IntegrationTests.Controllers.AuthorController
{
    internal class GetAmountAuthorControllerTests : GetAmountBaseLibraryControllerTests<Author, CreateAuthorRequest, AuthorResponse, LibraryFilterRequest>
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