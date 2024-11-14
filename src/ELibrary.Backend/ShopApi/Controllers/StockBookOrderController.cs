using Authentication.Identity;
using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Pagination;
using ShopApi.Features.StockBookOrderFeature.Command.CreateStockBookOrder;
using ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderAmount;
using ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderById;
using ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderPaginated;
using ShopApi.Features.StockBookOrderFeature.Dtos;

namespace ShopApi.Controllers
{
    [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
    [Route("stockbook")]
    [ApiController]
    public class StockBookOrderController : ControllerBase
    {
        private readonly IMediator mediator;

        public StockBookOrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StockBookOrderResponse>> GetStockOrderById(int id, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetStockOrderByIdQuery(id), cancellationToken);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }
        [HttpGet("amount")]
        [OutputCache(PolicyName = "StockBookOrderPaginationPolicy")]
        public async Task<ActionResult<int>> GetStockOrderAmount(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetStockOrderAmountQuery(), cancellationToken);
            return Ok(response);
        }
        [HttpPost("pagination")]
        [OutputCache(PolicyName = "StockBookOrderPaginationPolicy")]
        public async Task<ActionResult<IEnumerable<StockBookOrderResponse>>> GetStockOrderPaginated(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetStockOrderPaginatedQuery(paginationRequest), cancellationToken);
            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<StockBookOrderResponse>> CreateStockBookOrder(CreateStockBookOrderRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new CreateStockBookOrderCommand(request), cancellationToken);
            return Ok(response);
        }
    }
}