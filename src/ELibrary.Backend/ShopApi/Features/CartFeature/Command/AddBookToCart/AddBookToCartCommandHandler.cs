using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using MediatR;
using ShopApi.Features.CartFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.CartFeature.Command.AddBookToCart
{
    public class AddBookToCartCommandHandler : IRequestHandler<AddBookToCartCommand, CartBookResponse>
    {
        private readonly ICartService cartService;
        private readonly ILibraryService libraryService;
        private readonly IMapper mapper;

        public AddBookToCartCommandHandler(ICartService cartService, ILibraryService libraryService, IMapper mapper)
        {
            this.cartService = cartService;
            this.libraryService = libraryService;
            this.mapper = mapper;
        }

        public async Task<CartBookResponse> Handle(AddBookToCartCommand command, CancellationToken cancellationToken)
        {
            var cart = await cartService.GetCartByUserIdAsync(command.UserId, false, cancellationToken);

            if (cart == null)
            {
                cart = await cartService.CreateCartAsync(command.UserId, cancellationToken);
            }

            var cartBook = mapper.Map<CartBook>(command.Request);

            var bookResponse = await GetLibraryEntityHelper.
              GetBookResponsesForIdsAsync([cartBook.BookId],
              libraryService,
              cancellationToken);

            var response = await cartService.AddCartBookAsync(cart, cartBook, cancellationToken);

            var bookListingResponse = mapper.Map<CartBookResponse>(response);
            bookListingResponse.Book = bookResponse[0];

            return bookListingResponse;
        }
    }
}
