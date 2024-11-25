using LibraryApi.Domain.Dto.Author;
using LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.IntegrationTests.Controllers.AuthorController
{
    internal class CreateAuthorControllerTests : CreateBaseLibraryEntityControllerTests<Author, CreateAuthorRequest, AuthorResponse>
    {
        protected override string ControllerEndpoint => AuthorControllerTestHelper.ControllerEndpoint;

        protected override async Task<List<Author?>> CreateSamplesAsync()
        {
            return await AuthorControllerTestHelper.CreateSamplesAsync(CreateSampleEntityAsync);
        }

        protected override ValueTask<CreateAuthorRequest> GetCreateRequestAsync()
        {
            return ValueTask.FromResult(new CreateAuthorRequest()
            {
                Name = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1949, 6, 8, 0, 0, 0, DateTimeKind.Utc),
            });
        }

        protected override ValueTask<CreateAuthorRequest> GetInvalidCreateRequestAsync()
        {
            return ValueTask.FromResult(new CreateAuthorRequest());
        }
    }
}
