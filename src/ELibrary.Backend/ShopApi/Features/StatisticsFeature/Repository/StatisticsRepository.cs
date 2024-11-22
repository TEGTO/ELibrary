using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;
using ShopApi.Features.StatisticsFeature.Domain.Models;

namespace ShopApi.Features.StatisticsFeature.Repository
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly IDatabaseRepository<ShopDbContext> repository;

        public StatisticsRepository(IDatabaseRepository<ShopDbContext> repository)
        {
            this.repository = repository;
        }

        #region IStatisticsRepository Members

        public async Task<long> GetInCartCopiesAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Cart>(cancellationToken);
            queryable = GetQueryableWithIncludedBooks(queryable, getBookStatistics);
            return await queryable.SelectMany(cart => cart.Books).SumAsync(cartBook => cartBook.BookAmount, cancellationToken);
        }
        public async Task<long> GetInOrderCopiesAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await ApplyDateFilterToOrders(getBookStatistics, cancellationToken);
            queryable = GetQueryableWithIncludedBooks(queryable, getBookStatistics);
            return await queryable.SumAsync(o => o.OrderAmount, cancellationToken);
        }
        public async Task<long> GetSoldCopiesAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await ApplyDateFilterToOrders(getBookStatistics, cancellationToken);
            queryable = GetQueryableWithIncludedBooks(queryable, getBookStatistics);
            return await queryable.Where(x => x.OrderStatus == OrderStatus.Completed).SumAsync(o => o.OrderAmount, cancellationToken);
        }
        public async Task<long> GetCanceledCopiesAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await ApplyDateFilterToOrders(getBookStatistics, cancellationToken);
            queryable = GetQueryableWithIncludedBooks(queryable, getBookStatistics);
            return await queryable.Where(x => x.OrderStatus == OrderStatus.Canceled).SumAsync(o => o.OrderAmount, cancellationToken);
        }
        public async Task<long> GetOrderAmountAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await ApplyDateFilterToOrders(getBookStatistics, cancellationToken);
            queryable = GetQueryableWithIncludedBooks(queryable, getBookStatistics);
            return await queryable.CountAsync(cancellationToken);
        }
        public async Task<long> GetCanceledOrdersAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await ApplyDateFilterToOrders(getBookStatistics, cancellationToken);
            queryable = GetQueryableWithIncludedBooks(queryable, getBookStatistics);
            return await queryable.Where(x => x.OrderStatus == OrderStatus.Canceled).CountAsync(cancellationToken);
        }
        public async Task<decimal> GetAveragePriceAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await ApplyDateFilterToOrders(getBookStatistics, cancellationToken);
            queryable = GetQueryableWithIncludedBooks(queryable, getBookStatistics);
            return await queryable.AverageAsync(o => (decimal?)o.TotalPrice, cancellationToken) ?? 0;
        }
        public async Task<decimal> GetEarnedMoneyAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await ApplyDateFilterToOrders(getBookStatistics, cancellationToken);
            queryable = GetQueryableWithIncludedBooks(queryable, getBookStatistics);
            return await queryable.Where(x => x.OrderStatus == OrderStatus.Completed).SumAsync(o => o.TotalPrice, cancellationToken);
        }
        public async Task<Dictionary<DateTime, long>> GetOrderAmountInDaysAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await ApplyDateFilterToOrders(getBookStatistics, cancellationToken);
            queryable = GetQueryableWithIncludedBooks(queryable, getBookStatistics);
            return await queryable
                .GroupBy(order => order.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Date, x => (long)x.Count, cancellationToken);
        }

        #endregion

        #region Private Helpers

        private async Task<IQueryable<Order>> ApplyDateFilterToOrders(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Order>(cancellationToken);

            if (getBookStatistics.FromUTC.HasValue)
                queryable = queryable.Where(x => x.CreatedAt.ToUniversalTime() >= getBookStatistics.FromUTC.Value.ToUniversalTime());

            if (getBookStatistics.ToUTC.HasValue)
                queryable = queryable.Where(x => x.CreatedAt.ToUniversalTime() <= getBookStatistics.ToUTC.Value.ToUniversalTime());

            return queryable;
        }
        private static IQueryable<Order> GetQueryableWithIncludedBooks(IQueryable<Order> queryable, GetShopStatisticsFilter getBookStatistics)
        {
            if (getBookStatistics.IncludeBooks != null && getBookStatistics.IncludeBooks.Any())
            {
                var includeBookIds = getBookStatistics.IncludeBooks.Select(book => book.Id).ToArray();
                return queryable
                         .Where(order => order.OrderBooks.Any(orderBook => includeBookIds.Contains(orderBook.BookId)));
            }

            return queryable.AsSplitQuery();
        }
        private static IQueryable<Cart> GetQueryableWithIncludedBooks(IQueryable<Cart> queryable, GetShopStatisticsFilter getBookStatistics)
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
