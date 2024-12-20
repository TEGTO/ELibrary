﻿using Authentication.Identity;
using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using ShopApi.Features.CartFeature.Command.AddBookToCart;
using ShopApi.Features.CartFeature.Command.DeleteBooksFromCart;
using ShopApi.Features.CartFeature.Command.GetCart;
using ShopApi.Features.CartFeature.Command.GetInCartAmount;
using ShopApi.Features.CartFeature.Command.UpdateCartBookInCart;
using ShopApi.Features.CartFeature.Dtos;

namespace ShopApi.Controllers
{
    [Authorize(Policy = Policy.REQUIRE_CLIENT_ROLE)]
    [Route("cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator mediator;

        public CartController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        #region EndPoints

        [HttpGet]
        [OutputCache(PolicyName = "CartPolicy")]
        public async Task<ActionResult<CartResponse>> GetCart(CancellationToken cancellationToken)
        {
            var userId = Utilities.GetUserId(User);
            var response = await mediator.Send(new GetCartQuery(userId), cancellationToken);

            return Ok(response);
        }
        [HttpGet("amount")]
        [OutputCache(PolicyName = "CartPolicy")]
        public async Task<ActionResult<int>> GetInCartAmount(CancellationToken cancellationToken)
        {
            var userId = Utilities.GetUserId(User);
            var response = await mediator.Send(new GetInCartAmountQuery(userId), cancellationToken);

            return Ok(response);
        }
        [HttpPost("cartbook")]
        public async Task<ActionResult<CartBookResponse>> AddBookToCart(AddBookToCartRequest request, CancellationToken cancellationToken)
        {
            var userId = Utilities.GetUserId(User);
            var response = await mediator.Send(new AddBookToCartCommand(userId, request), cancellationToken);

            return Ok(response);
        }
        [HttpPut("cartbook")]
        public async Task<ActionResult<CartBookResponse>> UpdateCartBookInCart(UpdateCartBookRequest request, CancellationToken cancellationToken)
        {
            var userId = Utilities.GetUserId(User);
            var response = await mediator.Send(new UpdateCartBookInCartCommand(userId, request), cancellationToken);

            return Ok(response);
        }
        [HttpPut]
        public async Task<ActionResult<CartResponse>> DeleteBooksFromCart(List<DeleteCartBookFromCartRequest> requests, CancellationToken cancellationToken)
        {
            var userId = Utilities.GetUserId(User);
            var response = await mediator.Send(new DeleteBooksFromCartCommand(userId, requests), cancellationToken);

            return Ok(response);
        }

        #endregion
    }
}
