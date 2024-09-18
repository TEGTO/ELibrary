using Authentication.Identity;
using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using ShopApi.Domain.Dtos.Order;
using ShopApi.Services;
using System.Security.Claims;

namespace ShopApi.Controllers
{
    [Authorize]
    [Route("order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IClientService clientService;
        private readonly IOrderService orderService;

        public OrderController(IMapper mapper, IOrderService orderService, IClientService clientService)
        {
            this.mapper = mapper;
            this.orderService = orderService;
            this.clientService = clientService;
        }

        #region Endpoints

        [ResponseCache(Duration = 60)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders(CancellationToken cancellationToken)
        {
            var client = await GetClientAsync(cancellationToken);

            if (client == null)
            {
                return BadRequest("Client is not found!");
            }

            var orders = await orderService.GetOrdersByClientIdAsync(client.Id, cancellationToken);

            return Ok(orders.Select(mapper.Map<OrderResponse>));
        }
        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var client = await GetClientAsync(cancellationToken);

            if (client == null)
            {
                return BadRequest("Client is not found!");
            }

            var order = mapper.Map<Order>(request);
            order.ClientId = client.Id;

            var response = await orderService.CreateOrderAsync(order, cancellationToken);

            return Created($"", mapper.Map<OrderResponse>(response));
        }
        [HttpPatch]
        public async Task<ActionResult<OrderResponse>> UpdateOrder([FromBody] PatchOrderRequest request, CancellationToken cancellationToken)
        {
            var client = await GetClientAsync(cancellationToken);

            if (client == null || !await orderService.CheckOrderAsync(client.Id, request.Id, cancellationToken))
            {
                return BadRequest("Order is not found!");
            }

            var order = mapper.Map<Order>(request);
            var orderInService = await orderService.GetOrderByIdAsync(order.Id, cancellationToken);
            order.OrderStatus = orderInService!.OrderStatus;

            var response = await orderService.UpdateOrderAsync(order, cancellationToken);
            return Ok(mapper.Map<OrderResponse>(response));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id, CancellationToken cancellationToken)
        {
            var client = await GetClientAsync(cancellationToken);

            if (client == null || !await orderService.CheckOrderAsync(client.Id, id, cancellationToken))
            {
                return BadRequest("Order is not found!");
            }

            await orderService.DeleteOrderAsync(id, cancellationToken);
            return Ok();
        }

        #endregion

        #region Manager Endpoints

        [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
        [HttpPost("manager/pagination")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetPaginatedOrders(PaginationRequest request, CancellationToken cancellationToken)
        {
            var orders = await orderService.GetPaginatedAsync(request, cancellationToken);
            return Ok(orders.Select(mapper.Map<OrderResponse>));
        }
        [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
        [HttpPut("manager")]
        public async Task<ActionResult<OrderResponse>> ManagerUpdateOrder([FromBody] UpdateOrderRequest request, CancellationToken cancellationToken)
        {
            var order = mapper.Map<Order>(request);
            var response = await orderService.UpdateOrderAsync(order, cancellationToken);
            return Ok(mapper.Map<OrderResponse>(response));
        }
        [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
        [HttpDelete("manager/{id}")]
        public async Task<IActionResult> ManagerDeleteOrder(int id, CancellationToken cancellationToken)
        {
            await orderService.DeleteOrderAsync(id, cancellationToken);
            return Ok();
        }

        #endregion

        #region Private Helpers

        private async Task<Client?> GetClientAsync(CancellationToken cancellationToken)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await clientService.GetClientByUserIdAsync(userId, cancellationToken);
        }

        #endregion
    }
}
