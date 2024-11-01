using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using MediatR;
using ShopApi.Features.CartFeature.Services;

namespace ShopApi.Features.CartFeature.Command.AddBookToCart
{
    public class AddBookToCartCommandHandler : IRequestHandler<AddBookToCartCommand, BookListingResponse>
    {
        private readonly ICartService cartService;
        private readonly IMapper mapper;

        public AddBookToCartCommandHandler(ICartService cartService, IMapper mapper)
        {
            this.cartService = cartService;
            this.mapper = mapper;
        }

        public async Task<BookListingResponse> Handle(AddBookToCartCommand command, CancellationToken cancellationToken)
        {
            var cart = await cartService.GetCartByUserIdAsync(command.UserId, false, cancellationToken);

            if (cart == null)
            {
                cart = await cartService.CreateCartAsync(command.UserId, cancellationToken);
            }

            var cartBook = mapper.Map<CartBook>(command.Request);
            var response = await cartService.AddCartBookAsync(cart, cartBook, cancellationToken);
            return mapper.Map<BookListingResponse>(response);
        }
    }
}
