using MediatR;
using ShopApi.Features.CartFeature.Services;

namespace ShopApi.Features.CartFeature.Command.GetInCartAmount
{
    public class GetInCartAmountQueryHandler : IRequestHandler<GetInCartAmountQuery, int>
    {
        private readonly ICartService cartService;

        public GetInCartAmountQueryHandler(ICartService cartService)
        {
            this.cartService = cartService;
        }

        public async Task<int> Handle(GetInCartAmountQuery request, CancellationToken cancellationToken)
        {
            var cart = await cartService.GetCartByUserIdAsync(request.UserId, true, cancellationToken);

            if (cart == null)
            {
                cart = await cartService.CreateCartAsync(request.UserId, cancellationToken);
            }

            return await cartService.GetInCartAmountAsync(cart, cancellationToken);
        }
    }
}
