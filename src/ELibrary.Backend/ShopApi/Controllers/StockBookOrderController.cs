using Authentication.Identity;
using Caching;
using Caching.Services;
using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pagination;
using ShopApi.Features.StockBookOrderFeature.Command.CreateStockBookOrder;
using ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderAmount;
using ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderById;
using ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderPaginated;
using ShopApi.Features.StockBookOrderFeature.Dtos;
using System.Security.Claims;

namespace ShopApi.Controllers
{
    [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
    [Route("stockbook")]
    [ApiController]
    public class StockBookOrderController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICacheService cacheService;

        public StockBookOrderController(IMediator mediator, ICacheService cacheService)
        {
            this.mediator = mediator;
            this.cacheService = cacheService;
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
        public async Task<ActionResult<int>> GetStockOrderAmount(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cacheKey = $"GetStockOrderAmount_{userId}";
            var cachedResponse = await cacheService.GetDeserializedAsync<int?>(cacheKey);

            if (cachedResponse == null)
            {
                var response = await mediator.Send(new GetStockOrderAmountQuery(), cancellationToken);
                cachedResponse = response;

                await cacheService.SetSerializedAsync(cacheKey, cachedResponse, TimeSpan.FromSeconds(10));
            }

            return Ok(cachedResponse);
        }
        [HttpPost("pagination")]
        public async Task<ActionResult<IEnumerable<StockBookOrderResponse>>> GetStockOrderPaginated(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cacheKey = $"GetStockOrderPaginated_{userId}";
            var cachedResponse = await cacheService.GetDeserializedAsync<List<StockBookOrderResponse>>(cacheKey);

            if (cachedResponse == null)
            {
                var response = await mediator.Send(new GetStockOrderPaginatedQuery(paginationRequest), cancellationToken);
                cachedResponse = response.ToList();

                await cacheService.SetSerializedAsync(cacheKey, cachedResponse, TimeSpan.FromSeconds(10));
            }

            return Ok(cachedResponse);
        }
        [HttpPost]
        public async Task<ActionResult<StockBookOrderResponse>> CreateStockBookOrder(CreateStockBookOrderRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new CreateStockBookOrderCommand(request), cancellationToken);
            return Ok(response);
        }
    }
}