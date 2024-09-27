using Authentication.Identity;
using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using ShopApi.Features.StockBookOrderFeature.Dtos;
using ShopApi.Features.StockBookOrderFeature.Services;

namespace ShopApi.Controllers
{
    [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
    [Route("stockbook")]
    [ApiController]
    public class StockBookOrderController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IStockBookOrderService stockBookOrderService;

        public StockBookOrderController(IMapper mapper, IStockBookOrderService stockBookOrderService)
        {
            this.mapper = mapper;
            this.stockBookOrderService = stockBookOrderService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StockBookOrderResponse>> GetStockOrderById(int id, CancellationToken cancellationToken)
        {
            var order = await stockBookOrderService.GetStockBookOrderByIdAsync(id, cancellationToken);

            if (order == null)
            {
                return NotFound("Stock order is not found!");
            }

            return Ok(mapper.Map<StockBookOrderResponse>(order));
        }
        [HttpGet("amount")]
        public async Task<ActionResult<int>> GetStockOrderAmount(CancellationToken cancellationToken)
        {
            return Ok(await stockBookOrderService.GetStockBookAmountAsync(cancellationToken));
        }
        [HttpPost("pagination")]
        public async Task<ActionResult<IEnumerable<StockBookOrderResponse>>> GetStockOrderPaginated(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            var orders = await stockBookOrderService.GetPaginatedStockBookOrdersAsync(paginationRequest, cancellationToken);
            return Ok(orders.Select((mapper.Map<StockBookOrderResponse>)));
        }
        [HttpPost]
        public async Task<ActionResult<StockBookOrderResponse>> CreateStockBookOrder(CreateStockBookOrderRequest request, CancellationToken cancellationToken)
        {
            var order = mapper.Map<StockBookOrder>(request);
            return Created("", mapper.Map<StockBookOrderResponse>(await stockBookOrderService.AddStockBookOrderAsync(order, cancellationToken)));
        }
    }
}