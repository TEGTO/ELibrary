using Authentication.Identity;
using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using ShopApi.Features.CartFeature.Command.AddBookToCart;
using ShopApi.Features.CartFeature.Command.DeleteBooksFromCart;
using ShopApi.Features.CartFeature.Command.GetCart;
using ShopApi.Features.CartFeature.Command.GetInCartAmount;
using ShopApi.Features.CartFeature.Command.UpdateCartBookInCart;
using ShopApi.Features.CartFeature.Dtos;
using System.Security.Claims;

namespace ShopApi.Controllers
{
    [Authorize(Policy = Policy.REQUIRE_CLIENT_ROLE)]
    [Route("cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICacheService cacheService;

        public CartController(IMediator mediator, ICacheService cacheService)
        {
            this.mediator = mediator;
            this.cacheService = cacheService;
        }

        #region EndPoints

        [HttpGet]
        public async Task<ActionResult<CartResponse>> GetCart(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cacheKey = $"GetCart_{userId}";
            var cachedResponse = cacheService.Get<CartResponse>(cacheKey);

            if (cachedResponse == null)
            {
                var response = await mediator.Send(new GetCartQuery(userId), cancellationToken);
                cachedResponse = response;

                cacheService.Set(cacheKey, cachedResponse, TimeSpan.FromSeconds(3));
            }

            return Ok(cachedResponse);
        }
        [HttpGet("amount")]
        public async Task<ActionResult<int>> GetInCartAmount(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cacheKey = $"GetInCartAmount_{userId}";
            var cachedResponse = cacheService.Get<int?>(cacheKey);

            if (cachedResponse == null)
            {
                var response = await mediator.Send(new GetInCartAmountQuery(userId), cancellationToken);
                cachedResponse = response;

                cacheService.Set(cacheKey, cachedResponse, TimeSpan.FromSeconds(3));
            }

            return Ok(cachedResponse);
        }
        [HttpPost("cartbook")]
        public async Task<ActionResult<CartBookResponse>> AddBookToCart(AddBookToCartRequest request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await mediator.Send(new AddBookToCartCommand(userId, request), cancellationToken);

            return Ok(response);
        }
        [HttpPut("cartbook")]
        public async Task<ActionResult<CartBookResponse>> UpdateCartBookInCart(UpdateCartBookRequest request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await mediator.Send(new UpdateCartBookInCartCommand(userId, request), cancellationToken);

            return Ok(response);
        }
        [HttpPut]
        public async Task<ActionResult<CartResponse>> DeleteBooksFromCart(DeleteCartBookFromCartRequest[] requests, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await mediator.Send(new DeleteBooksFromCartCommand(userId, requests), cancellationToken);

            return Ok(response);
        }

        #endregion
    }
}
