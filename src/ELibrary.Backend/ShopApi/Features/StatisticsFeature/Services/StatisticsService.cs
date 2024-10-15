using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;
using ShopApi.Features.StatisticsFeature.Domain.Models;

namespace ShopApi.Features.StatisticsFeature.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IDatabaseRepository<LibraryShopDbContext> repository;

        public StatisticsService(IDatabaseRepository<LibraryShopDbContext> repository)
        {
            this.repository = repository;
        }

        #region IStatisticsServices Members

        public async Task<BookStatistics> GetStatisticsAsync(GetBookStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var inCartCopies = GetInCartCopiesAsync(getBookStatistics, cancellationToken);
            var inOrderCopies = GetInOrderCopiesAsync(getBookStatistics, cancellationToken);
            var soldCopies = GetSoldCopiesAsync(getBookStatistics, cancellationToken);
            var canceledCopies = GetCanceledCopiesAsync(getBookStatistics, cancellationToken);
            var orderAmount = GetOrderAmountAsync(getBookStatistics, cancellationToken);
            var canceledOrderAmount = GetCanceledOrdersAsync(getBookStatistics, cancellationToken);
            var averagePrice = GetAveragePriceAsync(getBookStatistics, cancellationToken);
            var stockAmount = GetStockAmountAsync(getBookStatistics, cancellationToken);
            var earnedMoney = GetEarnedMoneyAsync(getBookStatistics, cancellationToken);

            var tasks = new List<Task>
            {
                inCartCopies,
                inOrderCopies,
                soldCopies,
                canceledCopies,
                orderAmount,
                canceledOrderAmount,
                averagePrice,
                stockAmount,
                earnedMoney,
            };

            await Task.WhenAll(tasks);

            return new BookStatistics
            {
                InCartCopies = await inCartCopies,
                InOrderCopies = await inOrderCopies,
                SoldCopies = await soldCopies,
                CanceledCopies = await canceledCopies,
                OrderAmount = await orderAmount,
                CanceledOrderAmount = await canceledOrderAmount,
                AveragePrice = await averagePrice,
                StockAmount = await stockAmount,
                EarnedMoney = await earnedMoney
            };
        }

        #endregion

        #region Private Helpers

        private async Task<long> GetInCartCopiesAsync(GetBookStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Cart>(cancellationToken);

            queryable = QueryableWithIncludedBooks(queryable, getBookStatistics);

            return await queryable
                    .SelectMany(cart => cart.Books)
                    .SumAsync(cartBook => cartBook.BookAmount, cancellationToken);
        }
        private async Task<long> GetInOrderCopiesAsync(GetBookStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await QuearyableWithAppliedDateAsync(getBookStatistics, cancellationToken);

            queryable = QueryableWithIncludedBooks(queryable, getBookStatistics);

            return await queryable
                  .SelectMany(order => order.OrderBooks)
                  .SumAsync(orderBook => orderBook.BookAmount, cancellationToken);
        }
        private async Task<long> GetSoldCopiesAsync(GetBookStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await QuearyableWithAppliedDateAsync(getBookStatistics, cancellationToken);

            queryable = QueryableWithIncludedBooks(queryable, getBookStatistics);

            return await queryable
                   .Where(x => x.OrderStatus == OrderStatus.Completed)
                   .SelectMany(order => order.OrderBooks)
                   .SumAsync(orderBook => orderBook.BookAmount, cancellationToken);
        }
        private async Task<long> GetCanceledCopiesAsync(GetBookStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await QuearyableWithAppliedDateAsync(getBookStatistics, cancellationToken);

            queryable = QueryableWithIncludedBooks(queryable, getBookStatistics);

            return await queryable
                   .Where(x => x.OrderStatus == OrderStatus.Canceled)
                   .SelectMany(order => order.OrderBooks)
                   .SumAsync(orderBook => orderBook.BookAmount, cancellationToken);
        }
        private async Task<long> GetOrderAmountAsync(GetBookStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await QuearyableWithAppliedDateAsync(getBookStatistics, cancellationToken);

            queryable = QueryableWithIncludedBooks(queryable, getBookStatistics);

            return await queryable.CountAsync(cancellationToken);
        }
        private async Task<long> GetCanceledOrdersAsync(GetBookStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await QuearyableWithAppliedDateAsync(getBookStatistics, cancellationToken);

            queryable = QueryableWithIncludedBooks(queryable, getBookStatistics);

            return await queryable
                    .Where(x => x.OrderStatus == OrderStatus.Canceled).CountAsync(cancellationToken);
        }
        private async Task<decimal> GetAveragePriceAsync(GetBookStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);

            queryable = QueryableWithIncludedBooks(queryable, getBookStatistics);

            return await queryable
                               .AverageAsync(book => book.Price);
        }
        private async Task<long> GetStockAmountAsync(GetBookStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);

            queryable = QueryableWithIncludedBooks(queryable, getBookStatistics);

            return await queryable
                              .SumAsync(book => book.StockAmount);
        }
        private async Task<decimal> GetEarnedMoneyAsync(GetBookStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await QuearyableWithAppliedDateAsync(getBookStatistics, cancellationToken);

            queryable = QueryableWithIncludedBooks(queryable, getBookStatistics);

            return await queryable
                    .Where(x => x.OrderStatus == OrderStatus.Completed)
                    .SelectMany(order => order.OrderBooks)
                    .SumAsync(orderBook => orderBook.Book.Price * orderBook.BookAmount, cancellationToken);
        }

        private async Task<IQueryable<Order>> QuearyableWithAppliedDateAsync(GetBookStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);

            if (getBookStatistics.FromUTC.HasValue)
            {
                queryable = queryable.Where(x => x.CreatedAt >= getBookStatistics.FromUTC.Value);
            }

            if (getBookStatistics.ToUTC.HasValue)
            {
                queryable = queryable.Where(x => x.CreatedAt <= getBookStatistics.ToUTC.Value);
            }

            return queryable;
        }
        private IQueryable<Order> QueryableWithIncludedBooks(IQueryable<Order> queryable, GetBookStatistics getBookStatistics)
        {
            if (getBookStatistics.IncludeBooks != null && getBookStatistics.IncludeBooks.Any())
            {
                var includeBookIds = getBookStatistics.IncludeBooks.Select(book => book.Id).ToArray();
                return queryable
                         .Where(order => order.OrderBooks.Any(orderBook => includeBookIds.Contains(orderBook.BookId)));
            }

            return queryable.AsSplitQuery();
        }
        private IQueryable<Cart> QueryableWithIncludedBooks(IQueryable<Cart> queryable, GetBookStatistics getBookStatistics)
        {
            if (getBookStatistics.IncludeBooks != null && getBookStatistics.IncludeBooks.Any())
            {
                var includeBookIds = getBookStatistics.IncludeBooks.Select(book => book.Id).ToArray();
                return queryable
                     .Where(cartBook => cartBook.Books.Any(cartBook => includeBookIds.Contains(cartBook.BookId)));
            }

            return queryable.AsSplitQuery();
        }
        private IQueryable<Book> QueryableWithIncludedBooks(IQueryable<Book> queryable, GetBookStatistics getBookStatistics)
        {
            if (getBookStatistics.IncludeBooks != null && getBookStatistics.IncludeBooks.Any())
            {
                var includeBookIds = getBookStatistics.IncludeBooks.Select(book => book.Id).ToArray();
                return queryable
                      .Where(book => includeBookIds.Contains(book.Id));
            }

            return queryable.AsSplitQuery();
        }

        #endregion
    }
}
