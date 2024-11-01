using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using MediatR;
using ShopApi.Features.CartFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.CartFeature.Command.UpdateCartBookInCart
{
    public class UpdateCartBookInCartCommandHandler : IRequestHandler<UpdateCartBookInCartCommand, CartBookResponse>
    {
        private readonly ICartService cartService;
        private readonly ILibraryService libraryService;
        private readonly IMapper mapper;

        public UpdateCartBookInCartCommandHandler(ICartService cartService, ILibraryService libraryService, IMapper mapper)
        {
            this.cartService = cartService;
            this.libraryService = libraryService;
            this.mapper = mapper;
        }

        public async Task<CartBookResponse> Handle(UpdateCartBookInCartCommand command, CancellationToken cancellationToken)
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

            var bookResponse = await GetLibraryEntityHelper.
                GetBookResponsesForIdsAsync([cartBook.BookId],
                libraryService,
                cancellationToken);

            var response = await cartService.UpdateCartBookAsync(cart, cartBook, cancellationToken);

            var bookListingResponse = mapper.Map<CartBookResponse>(response);
            bookListingResponse.Book = bookResponse[0];

            return bookListingResponse;
        }
    }
}
