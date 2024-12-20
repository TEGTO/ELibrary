﻿using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using ShopApi.Services;

namespace ShopApi
{
    public static class GetLibraryEntityHelper
    {
        public static async Task<IEnumerable<BookResponse>> GetBookResponsesForIdsAsync(
            IEnumerable<int> ids,
            ILibraryService libraryService,
            CancellationToken cancellationToken)
        {
            if (ids == null || !ids.Any())
            {
                return new List<BookResponse>();
            }

            var distinctIds = ids.Distinct().ToList();

            var responses = await libraryService.GetByIdsAsync<BookResponse>(
                distinctIds,
                $"/{LibraryConfiguration.LIBRARY_API_GET_BOOKS_BY_IDS_ENDPOINT}",
                cancellationToken
            );

            if (responses.Count() != distinctIds.Count)
            {
                throw new InvalidDataException("Not all books were found!");
            }

            return responses;
        }

        public static async Task<CartResponse> GetCartResponseWithBooksAsync(Cart cart, ILibraryService libraryService, IMapper mapper, CancellationToken cancellationToken)
        {
            var bookIds = cart.Books.Select(x => x.BookId).Distinct();
            var bookResponses = await GetBookResponsesForIdsAsync(bookIds, libraryService, cancellationToken);
            var bookLookup = bookResponses.ToDictionary(book => book.Id);

            var cartResponse = mapper.Map<CartResponse>(cart);
            foreach (var cartBook in cartResponse.Books ?? [])
            {
                if (bookLookup.TryGetValue(cartBook.BookId, out var book))
                {
                    cartBook.Book = book;
                }
            }
            return cartResponse;
        }

        public static async Task<IEnumerable<OrderResponse>> GetOrderResponsesWithBooksAsync(IEnumerable<Order> orders, ILibraryService libraryService, IMapper mapper, CancellationToken cancellationToken)
        {
            var orderResponses = orders.Select(mapper.Map<OrderResponse>).ToList();

            var bookIds = GetDistinctBookIds(orderResponses);
            var bookResponses = await GetBookResponsesForIdsAsync(bookIds, libraryService, cancellationToken);

            var bookLookup = bookResponses.ToDictionary(book => book.Id);

            foreach (var order in orderResponses)
            {
                foreach (var orderBook in order.OrderBooks ?? Enumerable.Empty<OrderBookResponse>())
                {
                    if (bookLookup.TryGetValue(orderBook.BookId, out var book))
                    {
                        orderBook.Book = book;
                    }
                }
            }

            return orderResponses;
        }

        public static async Task<IEnumerable<StockBookOrderResponse>> GetStockBookOrderResponseWithBooksAsync(IEnumerable<StockBookOrder> orders, ILibraryService libraryService, IMapper mapper, CancellationToken cancellationToken)
        {
            var orderResponses = orders.Select(mapper.Map<StockBookOrderResponse>);

            var bookIds = GetDistinctBookIds(orderResponses);
            var bookResponses = await GetBookResponsesForIdsAsync(bookIds, libraryService, cancellationToken);
            var bookLookup = bookResponses.ToDictionary(book => book.Id);

            foreach (var order in orderResponses)
            {
                foreach (var stockBookChange in order.StockBookChanges ?? [])
                {
                    if (bookLookup.TryGetValue(stockBookChange.BookId, out var book))
                    {
                        stockBookChange.Book = book;
                    }
                }
            }

            return orderResponses;
        }

        #region Private Helpers

        private static IEnumerable<int> GetDistinctBookIds(IEnumerable<OrderResponse> orderResponses)
        {
            if (orderResponses == null) throw new ArgumentNullException(nameof(orderResponses));

            return orderResponses
                .Where(order => order.OrderBooks != null)
                .SelectMany(order => order.OrderBooks!)
                .Select(book => book.BookId)
                .Distinct();
        }
        private static IEnumerable<int> GetDistinctBookIds(IEnumerable<StockBookOrderResponse> orderResponses)
        {
            if (orderResponses == null) throw new ArgumentNullException(nameof(orderResponses));

            return orderResponses
                .Where(order => order.StockBookChanges != null)
                .SelectMany(order => order.StockBookChanges!)
                .Select(book => book.BookId)
                .Distinct();
        }

        #endregion
    }
}