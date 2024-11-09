using LibraryApi.Domain.Dto.Publisher;
using LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;

namespace LibraryApi.IntegrationTests.Controllers.PublisherController
{
    internal class GetPaginatedPublisherControllerTests : GetPaginatedBaseLibraryControllerTests<Publisher, CreatePublisherRequest, PublisherResponse, LibraryFilterRequest>
    {
        protected override string ControllerEndpoint => PublisherControllerTestHelper.ControllerEndpoint;

        protected override async Task<List<Publisher?>> CreateSamplesAsync()
        {
            return await PublisherControllerTestHelper.CreateSamplesAsync(CreateSampleEntityAsync);
        }

        protected override LibraryFilterRequest GetFilter()
        {
            return new LibraryFilterRequest();
        }
    }
}
