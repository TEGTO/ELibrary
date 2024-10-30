using LibraryApi.Domain.Dto.Publisher;
using LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.IntegrationTests.Controllers.PublisherController
{
    internal class CreatePublisherControllerTests : CreateBaseLibraryEntityControllerTests<Publisher, CreatePublisherRequest, PublisherResponse>
    {
        protected override string ControllerEndpoint => PublisherControllerTestHelper.ControllerEndpoint;

        protected override async Task<List<Publisher?>> CreateSamplesAsync()
        {
            return await PublisherControllerTestHelper.CreateSamplesAsync(CreateSampleEntityAsync);
        }

        protected override async ValueTask<CreatePublisherRequest> GetCreateRequestAsync()
        {
            return new CreatePublisherRequest()
            {
                Name = "Publisher",
            };
        }

        protected override async ValueTask<CreatePublisherRequest> GetInvalidCreateRequestAsync()
        {
            return new CreatePublisherRequest();
        }
    }
}
