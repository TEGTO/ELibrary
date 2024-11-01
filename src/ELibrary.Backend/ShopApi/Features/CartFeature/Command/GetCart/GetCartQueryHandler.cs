using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.CartFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.CartFeature.Command.GetCart
{
    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, CartResponse>
    {
        private readonly ICartService cartService;
        private readonly ILibraryService libraryService;
        private readonly IMapper mapper;

        public GetCartQueryHandler(ICartService cartService, ILibraryService libraryService, IMapper mapper)
        {
            this.cartService = cartService;
            this.libraryService = libraryService;
            this.mapper = mapper;
        }

        public async Task<CartResponse> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await cartService.GetCartByUserIdAsync(request.UserId, true, cancellationToken);

            if (cart == null)
            {
                cart = await cartService.CreateCartAsync(request.UserId, cancellationToken);
                return mapper.Map<CartResponse>(cart);
            }
            else
            {
                return await GetLibraryEntityHelper.GetCartResponseWiithBooksAsync(cart, libraryService, mapper, cancellationToken);
            }
        }
    }
}
