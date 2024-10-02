using Authentication.Identity;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using ShopApi.Features.ClientFeature.Services;
using ShopApi.Features.OrderFeature.Dtos;
using ShopApi.Features.OrderFeature.Services;
using System.Security.Claims;

namespace ShopApi.Controllers
{
    [Authorize(Policy = Policy.REQUIRE_CLIENT_ROLE)]
    [Route("order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderManager orderManager;
        private readonly IClientService clientService;

        public OrderController(
            IOrderManager orderCommandHandler,
            IClientService clientService)
        {
            this.orderManager = orderCommandHandler;
            this.clientService = clientService;
        }

        #region Endpoints

        [ResponseCache(Duration = 10)]
        [HttpPost("pagination")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders(PaginationRequest request, CancellationToken cancellationToken)
        {
            var client = await GetClientAsync(cancellationToken);

            if (client == null)
            {
                return BadRequest("Client is not found!");
            }

            var orders = await orderManager.GetPaginatedOrdersAsync(client.Id, request, cancellationToken);
            return Ok(orders);
        }
        [ResponseCache(Duration = 10)]
        [HttpGet("amount")]
        public async Task<ActionResult<int>> GetOrderAmount(CancellationToken cancellationToken)
        {
            var client = await GetClientAsync(cancellationToken);

            if (client == null)
            {
                return BadRequest("Client is not found!");
            }

            int amount = await orderManager.GetOrderAmountAsync(client.Id, cancellationToken);
            return Ok(amount);
        }
        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateOrder(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var client = await GetClientAsync(cancellationToken);

            if (client == null)
            {
                return BadRequest("Client is not found!");
            }

            var response = await orderManager.CreateOrderAsync(request, client, cancellationToken);
            return Created($"", response);
        }
        [HttpPatch]
        public async Task<ActionResult<OrderResponse>> UpdateOrder(ClientUpdateOrderRequest request, CancellationToken cancellationToken)
        {
            var client = await GetClientAsync(cancellationToken);

            if (client == null)
            {
                return BadRequest("Client is not found!");
            }

            var response = await orderManager.UpdateOrderAsync(request, client, cancellationToken);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelOrder(int id, CancellationToken cancellationToken)
        {
            var client = await GetClientAsync(cancellationToken);

            if (client == null)
            {
                return BadRequest("Client is not found!");
            }

            await orderManager.CancelOrderAsync(id, client, cancellationToken);
            return Ok();
        }

        #endregion

        #region Manager Endpoints

        [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
        [HttpGet("manager/{id}")]
        public async Task<ActionResult<OrderResponse>> ManagerGetOrderById(int id, CancellationToken cancellationToken)
        {
            var order = await orderManager.GetOrderByIdAsync(id, cancellationToken);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
        [ResponseCache(Duration = 10)]
        [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
        [HttpPost("manager/pagination")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> ManagerGetPaginatedOrders(PaginationRequest request, CancellationToken cancellationToken)
        {
            var orders = await orderManager.GetPaginatedOrdersAsync(request, cancellationToken);
            return Ok(orders);
        }
        [ResponseCache(Duration = 10)]
        [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
        [HttpGet("manager/amount")]
        public async Task<ActionResult<int>> ManagerGetOrderAmount(CancellationToken cancellationToken)
        {
            int amount = await orderManager.GetOrderAmountAsync(cancellationToken);
            return Ok(amount);
        }
        [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
        [HttpPut("manager")]
        public async Task<ActionResult<OrderResponse>> ManagerUpdateOrder(ManagerUpdateOrderRequest request, CancellationToken cancellationToken)
        {
            var response = await orderManager.UpdateOrderAsync(request, cancellationToken);
            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
        [HttpDelete("manager/{id}")]
        public async Task<IActionResult> ManagerCancelOrder(int id, CancellationToken cancellationToken)
        {
            await orderManager.CancelOrderAsync(id, cancellationToken);
            return Ok();
        }

        #endregion

        #region Private Helpers

        private async Task<Client?> GetClientAsync(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await clientService.GetClientByUserIdAsync(userId, cancellationToken);
        }

        #endregion
    }
}
