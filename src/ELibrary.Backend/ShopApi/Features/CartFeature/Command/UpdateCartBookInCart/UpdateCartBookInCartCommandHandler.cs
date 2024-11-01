using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using MediatR;
using ShopApi.Features.CartFeature.Services;

namespace ShopApi.Features.CartFeature.Command.UpdateCartBookInCart
{
    public class UpdateCartBookInCartCommandHandler : IRequestHandler<UpdateCartBookInCartCommand, BookListingResponse>
    {
        private readonly ICartService cartService;
        private readonly IMapper mapper;

        public UpdateCartBookInCartCommandHandler(ICartService cartService, IMapper mapper)
        {
            this.cartService = cartService;
            this.mapper = mapper;
        }

        public async Task<BookListingResponse> Handle(UpdateCartBookInCartCommand command, CancellationToken cancellationToken)
        {
            var cart = await cartService.GetCartByUserIdAsync(command.UserId, false, cancellationToken);

            if (cart == null)
            {
                cart = await cartService.CreateCartAsync(command.UserId, cancellationToken);
            }

            var cartBook = mapper.Map<CartBook>(command.Request);

            if (!await cartService.CheckBookCartAsync(cart, cartBook.Id, cancellationToken))
            {
                throw new InvalidDataException("Cart book is not found in the cart!");
            }

            var response = await cartService.UpdateCartBookAsync(cart, cartBook, cancellationToken);
            return mapper.Map<BookListingResponse>(response);
        }
    }
}
