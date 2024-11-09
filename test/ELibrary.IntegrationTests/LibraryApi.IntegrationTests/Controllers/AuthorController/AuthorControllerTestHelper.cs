using LibraryApi.Domain.Dto.Author;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.IntegrationTests.Controllers.AuthorController
{
    internal static class AuthorControllerTestHelper
    {
        public static string ControllerEndpoint => "author";

        public static async Task<List<Author?>> CreateSamplesAsync(Func<CreateAuthorRequest, Task<Author?>> createSampleEntityAsync)
        {
            var requests = new List<CreateAuthorRequest>
            {
               new CreateAuthorRequest { Name = "John", LastName = "Doe", DateOfBirth = new DateTime(1949, 6, 8, 0, 0, 0, DateTimeKind.Utc) },
               new CreateAuthorRequest { Name = "John2", LastName = "Doe2", DateOfBirth = new DateTime(1949, 6, 8, 0, 0, 0, DateTimeKind.Utc) },
            };

            var responseSlots = new List<Author?>
            {
                await createSampleEntityAsync(requests[0]),
                await createSampleEntityAsync(requests[1])
            };

            return responseSlots;
        }
    }
}