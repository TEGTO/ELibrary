using LibraryApi.Domain.Dto.Publisher;
using LibraryApi.Domain.Dtos;
using LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.IntegrationTests.Controllers.PublisherController
{
    internal class GetAmountPublisherControllerTests : GetAmountBaseLibraryControllerTests<Publisher, CreatePublisherRequest, PublisherResponse, LibraryFilterRequest>
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