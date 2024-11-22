using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.CartFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.CartFeature.Command.DeleteBooksFromCart
{
    public class DeleteBooksFromCartCommandHandler : IRequestHandler<DeleteBooksFromCartCommand, CartResponse>
    {
        private readonly ICartService cartService;
        private readonly ILibraryService libraryService;
        private readonly IMapper mapper;

        public DeleteBooksFromCartCommandHandler(ICartService cartService, ILibraryService libraryService, IMapper mapper)
        {
            this.cartService = cartService;
            this.libraryService = libraryService;
            this.mapper = mapper;
        }

        public async Task<CartResponse> Handle(DeleteBooksFromCartCommand command, CancellationToken cancellationToken)
        {
            var cart = await cartService.GetCartByUserIdAsync(command.UserId, false, cancellationToken);

            if (cart == null)
            {
                cart = await cartService.CreateCartAsync(command.UserId, cancellationToken);
            }

            var bookIds = command.Requests.Select(x => x.Id).Distinct();

            var bookResponses = await GetLibraryEntityHelper.GetBookResponsesForIdsAsync(bookIds.ToList(), libraryService, cancellationToken);

            var response = await cartService.DeleteBooksFromCartAsync(cart, bookIds.ToArray(), cancellationToken);

            var bookLookup = bookResponses.ToDictionary(book => book.Id);
            var cartResponse = mapper.Map<CartResponse>(response);
            foreach (var listingBook in cartResponse.Books ?? [])
            {
                if (bookLookup.TryGetValue(listingBook.BookId, out var book))
                {
                    listingBook.Book = book;
                }
            }
            return cartResponse;
        }
    }
}
