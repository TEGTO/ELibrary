using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Library;
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

        [ResponseCache(Duration = 1)]
        [HttpGet]
        public async Task<ActionResult<CartResponse>> GetCart(CancellationToken cancellationToken)
        {
            var cart = await GetCartAsync(true, cancellationToken);
            return Ok(mapper.Map<CartResponse>(cart));
        }
        [ResponseCache(Duration = 1)]
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
        [HttpPut]
        public async Task<ActionResult<CartResponse>> DeleteBooksFromCart(DeleteCartBookFromCartRequest[] requests, CancellationToken cancellationToken)
        {
            var books = requests.Select(mapper.Map<Book>);
            var cart = await GetCartAsync(false, cancellationToken);
            var response = await cartService.DeleteBooksFromCartAsync(cart, books.ToArray(), cancellationToken);
            return Ok(mapper.Map<CartResponse>(response));
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
