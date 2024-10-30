using LibraryApi.Domain.Dto.Publisher;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.IntegrationTests.Controllers.PublisherController
{
    internal static class PublisherControllerTestHelper
    {
        public static string ControllerEndpoint => "publisher";

        public static async Task<List<Publisher?>> CreateSamplesAsync(Func<CreatePublisherRequest, Task<Publisher?>> createSampleEntityAsync)
        {
            var requests = new List<CreatePublisherRequest>
            {
               new CreatePublisherRequest { Name = "Publisher" },
               new CreatePublisherRequest { Name = "Publisher2"},
            };

            var responseSlots = new List<Publisher?>
            {
                await createSampleEntityAsync(requests[0]),
                await createSampleEntityAsync(requests[1])
            };

            return responseSlots;
        }
    }
}