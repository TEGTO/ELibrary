using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Features.CartFeature.Dtos;
using ShopApi.Features.CartFeature.Services;
using ShopApi.Features.ClientFeature.Services;
using System.Security.Claims;

namespace ShopApi.Controllers
{
    [Authorize]
    [Route("cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IClientService clientService;
        private readonly ICartService cartService;

        public CartController(IMapper mapper, IClientService clientService, ICartService cartService)
        {
            this.mapper = mapper;
            this.clientService = clientService;
            this.cartService = cartService;
        }

        #region EndPoints

        [HttpGet]
        public async Task<ActionResult<CartResponse>> GetCart(CancellationToken cancellationToken)
        {
            var cart = await GetCartAsync(true, cancellationToken);
            return Ok(mapper.Map<CartResponse>(cart));
        }
        [HttpGet("amount")]
        public async Task<ActionResult<CartResponse>> GetInCartAmount(CancellationToken cancellationToken)
        {
            var cart = await GetCartAsync(true, cancellationToken);
            int amount = await cartService.GetInCartAmountAsync(cart, cancellationToken);
            return Ok(amount);
        }
        [HttpPost("cartbook")]
        public async Task<ActionResult<BookListingResponse>> AddBookToCart(AddBookToCartRequest request, CancellationToken cancellationToken)
        {
            var cart = await GetCartAsync(false, cancellationToken);
            var cartBook = mapper.Map<CartBook>(request);
            var response = await cartService.AddCartBookAsync(cart, cartBook, cancellationToken);
            return Ok(mapper.Map<BookListingResponse>(response));
        }
        [HttpPut("cartbook")]
        public async Task<ActionResult<BookListingResponse>> UpdateCartBookInCart(UpdateCartBookRequest request, CancellationToken cancellationToken)
        {
            var cart = await GetCartAsync(false, cancellationToken);
            var cartBook = mapper.Map<CartBook>(request);

            if (!await cartService.CheckBookCartAsync(cart, cartBook.Id, cancellationToken))
            {
                return BadRequest("Cart book is not found in the cart!");
            }

            var response = await cartService.UpdateCartBookAsync(cart, cartBook, cancellationToken);
            return Ok(mapper.Map<BookListingResponse>(response));
        }
        [HttpDelete("cartbook/{id}")]
        public async Task<IActionResult> DeleteCartBookFromCart(string id, CancellationToken cancellationToken)
        {
            var cart = await GetCartAsync(false, cancellationToken);

            if (!await cartService.CheckBookCartAsync(cart, id, cancellationToken))
            {
                return BadRequest("Cart book is not found in the cart!");
            }

            await cartService.DeleteCartBookAsync(cart, id, cancellationToken);
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult<CartResponse>> ClearCart(CancellationToken cancellationToken)
        {
            var cart = await GetCartAsync(false, cancellationToken);
            var response = await cartService.ClearCartAsync(cart, cancellationToken);
            return Ok(mapper.Map<CartResponse>(cart));
        }

        #endregion

        #region Private Helpers

        private async Task<Cart> GetCartAsync(bool includeProducts, CancellationToken cancellationToken)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await cartService.GetCartByUserIdAsync(userId, includeProducts, cancellationToken);

            if (cart == null)
            {
                cart = await cartService.CreateCartAsync(userId, cancellationToken);
            }

            return cart;
        }

        #endregion
    }
}
