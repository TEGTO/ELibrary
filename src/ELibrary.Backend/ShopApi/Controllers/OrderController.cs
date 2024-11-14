using Authentication.Identity;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using ShopApi.Features.OrderFeature.Command.CancelOrder;
using ShopApi.Features.OrderFeature.Command.CreateOrder;
using ShopApi.Features.OrderFeature.Command.GetOrderAmount;
using ShopApi.Features.OrderFeature.Command.GetOrders;
using ShopApi.Features.OrderFeature.Command.ManagerCancelOrder;
using ShopApi.Features.OrderFeature.Command.ManagerGetOrderAmount;
using ShopApi.Features.OrderFeature.Command.ManagerGetOrderById;
using ShopApi.Features.OrderFeature.Command.ManagerGetPaginatedOrders;
using ShopApi.Features.OrderFeature.Command.ManagerUpdateOrder;
using ShopApi.Features.OrderFeature.Command.UpdateOrder;
using ShopApi.Features.OrderFeature.Dtos;
using System.Security.Claims;

namespace ShopApi.Controllers
{
    [Authorize(Policy = Policy.REQUIRE_CLIENT_ROLE)]
    [Route("order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        #region Endpoints

        [HttpPost("pagination")]
        [OutputCache(PolicyName = "OrderPaginationPolicy")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders(GetOrdersFilter request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await mediator.Send(new GetOrdersQuery(userId, request), cancellationToken);

            return Ok(response);
        }
        [HttpPost("amount")]
        [OutputCache(PolicyName = "OrderPaginationPolicy")]
        public async Task<ActionResult<int>> GetOrderAmount(GetOrdersFilter request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await mediator.Send(new GetOrderAmountQuery(userId, request), cancellationToken);

            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateOrder(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await mediator.Send(new CreateOrderCommand(userId, request), cancellationToken);

            return Created($"", response);
        }
        [HttpPatch]
        public async Task<ActionResult<OrderResponse>> UpdateOrder(ClientUpdateOrderRequest request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await mediator.Send(new UpdateOrderCommand(userId, request), cancellationToken);

            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelOrder(int id, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await mediator.Send(new CancelOrderCommand(userId, id), cancellationToken);

            return Ok();
        }

        #endregion

        #region Manager Endpoints

        [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
        [HttpGet("manager/{id}")]
        public async Task<ActionResult<OrderResponse>> ManagerGetOrderById(int id, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new ManagerGetOrderByIdQuery(id), cancellationToken);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
        [HttpPost("manager/pagination")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> ManagerGetPaginatedOrders(GetOrdersFilter request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new ManagerGetPaginatedOrdersQuery(request), cancellationToken);
            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
        [HttpPost("manager/amount")]
        public async Task<ActionResult<int>> ManagerGetOrderAmount(GetOrdersFilter request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new ManagerGetOrderAmountQuery(request), cancellationToken);
            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
        [HttpPut("manager")]
        public async Task<ActionResult<OrderResponse>> ManagerUpdateOrder(ManagerUpdateOrderRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new ManagerUpdateOrderCommand(request), cancellationToken);
            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
        [HttpDelete("manager/{id}")]
        public async Task<IActionResult> ManagerCancelOrder(int id, CancellationToken cancellationToken)
        {
            await mediator.Send(new ManagerCancelOrderCommand(id), cancellationToken);
            return Ok();
        }

        #endregion
    }
}
