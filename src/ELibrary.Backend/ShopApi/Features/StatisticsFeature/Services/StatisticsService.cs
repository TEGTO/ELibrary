using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;
using ShopApi.Features.StatisticsFeature.Domain.Models;

namespace ShopApi.Features.StatisticsFeature.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IDatabaseRepository<ShopDbContext> repository;

        public StatisticsService(IDatabaseRepository<ShopDbContext> repository)
        {
            this.repository = repository;
        }

        #region IStatisticsServices Members

        public async Task<ShopStatistics> GetStatisticsAsync(GetShopStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var inCartCopies = GetInCartCopiesAsync(getBookStatistics, cancellationToken);
            var inOrderCopies = GetInOrderCopiesAsync(getBookStatistics, cancellationToken);
            var soldCopies = GetSoldCopiesAsync(getBookStatistics, cancellationToken);
            var canceledCopies = GetCanceledCopiesAsync(getBookStatistics, cancellationToken);
            var orderAmount = GetOrderAmountAsync(getBookStatistics, cancellationToken);
            var canceledOrderAmount = GetCanceledOrdersAsync(getBookStatistics, cancellationToken);
            var averagePrice = GetAveragePriceAsync(getBookStatistics, cancellationToken);
            var earnedMoney = GetEarnedMoneyAsync(getBookStatistics, cancellationToken);
            var orderAmountInDays = GetOrderAmountInDaysAsync(getBookStatistics, cancellationToken);

            var tasks = new List<Task>
            {
                inCartCopies,
                inOrderCopies,
                soldCopies,
                canceledCopies,
                orderAmount,
                canceledOrderAmount,
                averagePrice,
                earnedMoney,
                orderAmountInDays
            };

            await Task.WhenAll(tasks);

            return new ShopStatistics
            {
                InCartCopies = await inCartCopies,
                InOrderCopies = await inOrderCopies,
                SoldCopies = await soldCopies,
                CanceledCopies = await canceledCopies,
                OrderAmount = await orderAmount,
                CanceledOrderAmount = await canceledOrderAmount,
                AveragePrice = await averagePrice,
                EarnedMoney = await earnedMoney,
                OrderAmountInDays = await orderAmountInDays
            };
        }

        #endregion

        #region Private Helpers

        private async Task<long> GetInCartCopiesAsync(GetShopStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Cart>(cancellationToken);

            queryable = GetCartQueryableWithIncludedBooks(queryable, getBookStatistics);

            return await queryable
                    .SelectMany(cart => cart.Books)
                    .SumAsync(cartBook => cartBook.BookAmount, cancellationToken);
        }
        private async Task<long> GetInOrderCopiesAsync(GetShopStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await OrderQuearyableWithAppliedDateAsync(getBookStatistics, cancellationToken);

            queryable = GetOrderQueryableWithIncludedBooks(queryable, getBookStatistics);

            return await queryable
                  .SelectMany(order => order.OrderBooks)
                  .SumAsync(orderBook => orderBook.BookAmount, cancellationToken);
        }
        private async Task<long> GetSoldCopiesAsync(GetShopStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await OrderQuearyableWithAppliedDateAsync(getBookStatistics, cancellationToken);

            queryable = GetOrderQueryableWithIncludedBooks(queryable, getBookStatistics);

            return await queryable
                   .Where(x => x.OrderStatus == OrderStatus.Completed)
                   .SelectMany(order => order.OrderBooks)
                   .SumAsync(orderBook => orderBook.BookAmount, cancellationToken);
        }
        private async Task<long> GetCanceledCopiesAsync(GetShopStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await OrderQuearyableWithAppliedDateAsync(getBookStatistics, cancellationToken);

            queryable = GetOrderQueryableWithIncludedBooks(queryable, getBookStatistics);

            return await queryable
                   .Where(x => x.OrderStatus == OrderStatus.Canceled)
                   .SelectMany(order => order.OrderBooks)
                   .SumAsync(orderBook => orderBook.BookAmount, cancellationToken);
        }
        private async Task<long> GetOrderAmountAsync(GetShopStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await OrderQuearyableWithAppliedDateAsync(getBookStatistics, cancellationToken);

            queryable = GetOrderQueryableWithIncludedBooks(queryable, getBookStatistics);

            return await queryable.CountAsync(cancellationToken);
        }
        private async Task<long> GetCanceledOrdersAsync(GetShopStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await OrderQuearyableWithAppliedDateAsync(getBookStatistics, cancellationToken);

            queryable = GetOrderQueryableWithIncludedBooks(queryable, getBookStatistics);

            return await queryable
                    .Where(x => x.OrderStatus == OrderStatus.Canceled).CountAsync(cancellationToken);
        }
        private async Task<decimal> GetAveragePriceAsync(GetShopStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await OrderQuearyableWithAppliedDateAsync(getBookStatistics, cancellationToken);

            queryable = GetOrderQueryableWithIncludedBooks(queryable, getBookStatistics);

            return await queryable
                   .AverageAsync(o => (decimal?)o.TotalPrice, cancellationToken) ?? 0;
        }
        private async Task<decimal> GetEarnedMoneyAsync(GetShopStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await OrderQuearyableWithAppliedDateAsync(getBookStatistics, cancellationToken);

            queryable = GetOrderQueryableWithIncludedBooks(queryable, getBookStatistics);

            return await queryable
                    .Where(x => x.OrderStatus == OrderStatus.Completed)
                    .SumAsync(o => o.TotalPrice, cancellationToken);
        }
        private async Task<Dictionary<DateTime, long>> GetOrderAmountInDaysAsync(GetShopStatistics getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await OrderQuearyableWithAppliedDateAsync(getBookStatistics, cancellationToken);

            queryable = GetOrderQueryableWithIncludedBooks(queryable, getBookStatistics);

            var result = await queryable
              .AsSplitQuery()
              .GroupBy(order => order.CreatedAt.Date)
              .Select(g => new { Date = g.Key, Count = g.Count() })
              .OrderBy(x => x.Date)
              .ToDictionaryAsync(x => x.Date, x => (long)x.Count, cancellationToken);

            return result;
        }

        private async Task<IQueryable<Order>> OrderQuearyableWithAppliedDateAsync(GetShopStatistics getBookStatistics, CancellationToken cancellationToken)
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
        private IQueryable<Order> GetOrderQueryableWithIncludedBooks(IQueryable<Order> queryable, GetShopStatistics getBookStatistics)
        {
            if (getBookStatistics.IncludeBooks != null && getBookStatistics.IncludeBooks.Any())
            {
                var includeBookIds = getBookStatistics.IncludeBooks.Select(book => book.Id).ToArray();
                return queryable
                         .Where(order => order.OrderBooks.Any(orderBook => includeBookIds.Contains(orderBook.BookId)));
            }

            return queryable.AsSplitQuery();
        }
        private IQueryable<Cart> GetCartQueryableWithIncludedBooks(IQueryable<Cart> queryable, GetShopStatistics getBookStatistics)
        {
            if (getBookStatistics.IncludeBooks != null && getBookStatistics.IncludeBooks.Any())
            {
                var includeBookIds = getBookStatistics.IncludeBooks.Select(book => book.Id).ToArray();
                return queryable
                     .Where(cartBook => cartBook.Books.Any(cartBook => includeBookIds.Contains(cartBook.BookId)));
            }

            return queryable.AsSplitQuery();
        }

        #endregion
    }
}
