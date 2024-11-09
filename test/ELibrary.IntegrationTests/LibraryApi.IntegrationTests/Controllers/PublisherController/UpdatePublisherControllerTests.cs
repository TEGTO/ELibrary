using LibraryApi.Domain.Dto.Publisher;
using LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.IntegrationTests.Controllers.PublisherController
{
    internal class UpdatePublisherControllerTests : UpdateBaseLibraryEntityControllerTests<Publisher, CreatePublisherRequest, PublisherResponse, UpdatePublisherRequest>
    {
        protected override string ControllerEndpoint => PublisherControllerTestHelper.ControllerEndpoint;

        protected override async Task<List<Publisher?>> CreateSamplesAsync()
        {
            return await PublisherControllerTestHelper.CreateSamplesAsync(CreateSampleEntityAsync);
        }

        protected override UpdatePublisherRequest GetUpdateRequest()
        {
            return new UpdatePublisherRequest()
            {
                Id = 1,
                Name = "NewPublisherName",
            };
        }

        protected override UpdatePublisherRequest GetInvalidUpdateRequest()
        {
            return new UpdatePublisherRequest();
        }
    }
}
