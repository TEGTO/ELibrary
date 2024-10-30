using LibraryApi.Domain.Dto.Author;
using LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.IntegrationTests.Controllers.AuthorController
{
    internal class UpdateAuthorControllerTests : UpdateBaseLibraryEntityControllerTests<Author, CreateAuthorRequest, AuthorResponse, UpdateAuthorRequest>
    {
        protected override string ControllerEndpoint => AuthorControllerTestHelper.ControllerEndpoint;

        protected override async Task<List<Author?>> CreateSamplesAsync()
        {
            return await AuthorControllerTestHelper.CreateSamplesAsync(CreateSampleEntityAsync);
        }

        protected override UpdateAuthorRequest GetUpdateRequest()
        {
            return new UpdateAuthorRequest()
            {
                Id = 1,
                Name = "NewName",
                LastName = "Doe",
                DateOfBirth = new DateTime(1949, 6, 8, 0, 0, 0, DateTimeKind.Utc),
            };
        }

        protected override UpdateAuthorRequest GetInvalidUpdateRequest()
        {
            return new UpdateAuthorRequest();
        }
    }
}
