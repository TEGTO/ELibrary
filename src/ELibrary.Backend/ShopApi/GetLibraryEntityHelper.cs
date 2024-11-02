using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using ShopApi.Services;

namespace ShopApi
{
    public static class GetLibraryEntityHelper
    {
        public static async Task<List<BookResponse>> GetBookResponsesForIdsAsync(List<int> ids, ILibraryService libraryService, CancellationToken cancellationToken)
        {
            var responses = (await libraryService.GetByIdsAsync<BookResponse>(
                ids,
                $"/{LibraryConfiguration.LIBRARY_API_GET_BOOKS_BY_IDS_ENDPOINT}",
                cancellationToken
            )).ToList();

            if (responses.Count != ids.Distinct().Count())
            {
                throw new InvalidDataException($"Not all books exist!");
            }

            return responses;
        }

        public static async Task<CartResponse> GetCartResponseWiithBooksAsync(Cart cart, ILibraryService libraryService, IMapper mapper, CancellationToken cancellationToken)
        {
            var bookIds = cart.Books.Select(x => x.BookId).Distinct().ToList();
            var bookResponses = await GetBookResponsesForIdsAsync(bookIds, libraryService, cancellationToken);
            var bookLookup = bookResponses.ToDictionary(book => book.Id);

            var cartResponse = mapper.Map<CartResponse>(cart);
            foreach (var cartBook in cartResponse.Books)
            {
                if (bookLookup.TryGetValue(cartBook.BookId, out var book))
                {
                    cartBook.Book = book;
                }
            }
            return cartResponse;
        }
        public static async Task<IEnumerable<OrderResponse>> GetOrderResponsesWiithBooksAsync(IEnumerable<Order> orders, ILibraryService libraryService, IMapper mapper, CancellationToken cancellationToken)
        {
            var orderResponses = orders.Select(mapper.Map<OrderResponse>).ToList();

            var bookIds = orderResponses.SelectMany(order => order.OrderBooks.Select(book => book.BookId)).Distinct().ToList();
            var bookResponses = await GetBookResponsesForIdsAsync(bookIds, libraryService, cancellationToken);
            var bookLookup = bookResponses.ToDictionary(book => book.Id);

            foreach (var order in orderResponses)
            {
                foreach (var orderBook in order.OrderBooks)
                {
                    if (bookLookup.TryGetValue(orderBook.BookId, out var book))
                    {
                        orderBook.Book = book;
                    }
                }
            }

            return orderResponses;
        }
        public static async Task<IEnumerable<StockBookOrderResponse>> GetStockBookOrderResponseWiithBooksAsync(IEnumerable<StockBookOrder> orders, ILibraryService libraryService, IMapper mapper, CancellationToken cancellationToken)
        {
            var orderResponses = orders.Select(mapper.Map<StockBookOrderResponse>).ToList();

            var bookIds = orderResponses.SelectMany(order => order.StockBookChanges.Select(book => book.BookId)).Distinct().ToList();
            var bookResponses = await GetBookResponsesForIdsAsync(bookIds, libraryService, cancellationToken);
            var bookLookup = bookResponses.ToDictionary(book => book.Id);

            foreach (var order in orderResponses)
            {
                foreach (var stockBookChange in order.StockBookChanges)
                {
                    if (bookLookup.TryGetValue(stockBookChange.BookId, out var book))
                    {
                        stockBookChange.Book = book;
                    }
                }
            }

            return orderResponses;
        }
    }
}