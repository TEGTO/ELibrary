using Authentication.Identity;
using Authentication.Token;
using LibraryShopEntities.Domain.Dtos.Library;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ShopApi.IntegrationTests.Controllers.OrderController
{
    internal class BaseOrderControllerTest : BaseControllerTest
    {
        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            await AddOrdersToClientAsync(AccessToken);

            mockLibraryService.Setup(x => x.GetByIdsAsync<BookResponse>(
                It.IsAny<List<int>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync((List<int> ids, string endpoint, CancellationToken token) =>
            {
                var list = new List<BookResponse>();

                foreach (var id in ids)
                {
                    list.Add(new BookResponse { Id = id, Name = $"Test{id}" });
                }

                return list;
            });

        }

        protected async Task AddOrdersToClientAsync(string accessToken)
        {
            var client = await TestHelper.CreateClientAsync(AccessToken, httpClient);
            await GetPostgreSqlContainer().ExecScriptAsync(
                $"INSERT INTO public.orders (id, created_at, updated_at, order_amount, total_price, contact_client_name, contact_phone, delivery_address, delivery_time, order_status, payment_method, delivery_method, client_id) VALUES(1, '2024-11-02 14:20:58.870', '2024-11-02 14:20:58.871', 1, 500.00, 'client name', '0123456789', '2', '2024-11-03 06:30:00.000', 0, 0, 0, '{client.Id}');" +
                $"INSERT INTO public.orders (id, created_at, updated_at, order_amount, total_price, contact_client_name, contact_phone, delivery_address, delivery_time, order_status, payment_method, delivery_method, client_id) VALUES(2, '2024-10-02 14:20:58.870', '2024-10-02 14:20:58.871', 1, 500.00, 'client name', '0123456789', '2', '2024-10-03 06:30:00.000', -1, 0, 0, '{client.Id}');" +
                $"INSERT INTO public.order_books (id, book_amount, book_id, book_price, order_id) VALUES('0b0b8823-99dd-4cb2-809c-757b4d36ce0d', 1, 1, 500.00, 1);" +
                $"INSERT INTO public.order_books (id, book_amount, book_id, book_price, order_id) VALUES('1b0b8823-99dd-4cb2-809c-757b4d36ce0d', 1, 1, 500.00, 2);"
                );
        }
        protected async Task<string> GenerateNewClientAccessTokenAsync()
        {
            var jwtHandler = new JwtHandler(settings);
            var random = new Random();
            IdentityUser identity = new IdentityUser()
            {
                Id = random.Next(int.MaxValue).ToString(),
                UserName = $"testuser{random.Next(int.MaxValue).ToString()}",
                Email = $"test{random.Next(int.MaxValue).ToString()}@example.com"
            };

            var accessToken = jwtHandler.CreateToken(identity, [Roles.CLIENT]).AccessToken;

            await TestHelper.CreateClientAsync(accessToken, httpClient);

            return accessToken;
        }
    }
}
