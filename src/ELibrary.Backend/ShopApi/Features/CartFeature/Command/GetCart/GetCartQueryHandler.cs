using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.CartFeature.Services;

namespace ShopApi.Features.CartFeature.Command.GetCart
{
    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, CartResponse>
    {
        private readonly ICartService cartService;
        private readonly IMapper mapper;

        public GetCartQueryHandler(ICartService cartService, IMapper mapper)
        {
            this.cartService = cartService;
            this.mapper = mapper;
        }

        public async Task<CartResponse> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await cartService.GetCartByUserIdAsync(request.UserId, true, cancellationToken);

            if (cart == null)
            {
                cart = await cartService.CreateCartAsync(request.UserId, cancellationToken);
            }

            return mapper.Map<CartResponse>(cart);
        }
    }
}
