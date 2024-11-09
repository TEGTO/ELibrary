using LibraryApi.Domain.Dto.Genre;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.IntegrationTests.Controllers.GenreController
{
    internal static class GenreControllerTestHelper
    {
        public static string ControllerEndpoint => "genre";

        public static async Task<List<Genre?>> CreateSamplesAsync(Func<CreateGenreRequest, Task<Genre?>> createSampleEntityAsync)
        {
            var requests = new List<CreateGenreRequest>
            {
               new CreateGenreRequest { Name = "Genre" },
               new CreateGenreRequest { Name = "Genre2"},
            };

            var responseSlots = new List<Genre?>
            {
                await createSampleEntityAsync(requests[0]),
                await createSampleEntityAsync(requests[1])
            };

            return responseSlots;
        }
    }
}