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

        protected override ValueTask<CreatePublisherRequest> GetCreateRequestAsync()
        {
            return ValueTask.FromResult(new CreatePublisherRequest()
            {
                Name = "Publisher",
            });
        }

        protected override ValueTask<CreatePublisherRequest> GetInvalidCreateRequestAsync()
        {
            return ValueTask.FromResult(new CreatePublisherRequest());
        }
    }
}
