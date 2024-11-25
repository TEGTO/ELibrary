using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;
using System.Net;
using System.Text;
using System.Text.Json;

namespace LibraryApi.IntegrationTests.Controllers.BaseLibraryEntityController
{
    internal abstract class GetAmountBaseLibraryControllerTests<
        TEntity,
        TCreateRequest,
        TEntityResponse,
        TFilter
        > : BaseLibraryEntityControllerTest<TEntity, TCreateRequest, TEntityResponse> where TEntity : BaseLibraryEntity where TFilter : LibraryFilterRequest
    {
        protected abstract TFilter GetFilter();

        [Test]
        public async Task GetEntityAmount_ReturnsOkWithAmount()
        {
            // Arrange
            var list = await CreateSamplesAsync();
            var filter = GetFilter();

            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/amount");
            request.Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");

            // Act 
            var response = await client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            Assert.That(int.Parse(content), Is.EqualTo(list.Count));
        }

        [Test]
        public async Task GetEntityAmount_ReturnsOkWithZeroAmount()
        {
            // Arrange
            var filter = GetFilter();
            filter.ContainsName = "ZEROMATCHES"
                ;
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ControllerEndpoint}/amount");
            request.Content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");

            // Act 
            var response = await client.SendAsync(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            Assert.That(int.Parse(content), Is.EqualTo(0));
        }
    }
}
