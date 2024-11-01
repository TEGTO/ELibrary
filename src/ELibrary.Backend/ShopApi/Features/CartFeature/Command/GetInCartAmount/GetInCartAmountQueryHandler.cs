using AutoMapper;
using MediatR;
using ShopApi.Features.CartFeature.Services;

namespace ShopApi.Features.CartFeature.Command.GetInCartAmount
{
    public class GetInCartAmountQueryHandler : IRequestHandler<GetInCartAmountQuery, int>
    {
        private readonly ICartService cartService;
        private readonly IMapper mapper;

        public GetInCartAmountQueryHandler(ICartService cartService, IMapper mapper)
        {
            this.cartService = cartService;
            this.mapper = mapper;
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
