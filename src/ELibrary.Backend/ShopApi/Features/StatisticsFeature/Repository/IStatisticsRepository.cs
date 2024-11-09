using ShopApi.Features.StatisticsFeature.Domain.Models;

namespace ShopApi.Features.StatisticsFeature.Repository
{
    public interface IStatisticsRepository
    {
        public Task<decimal> GetAveragePriceAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken);
        public Task<long> GetCanceledCopiesAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken);
        public Task<long> GetCanceledOrdersAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken);
        public Task<decimal> GetEarnedMoneyAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken);
        public Task<long> GetInCartCopiesAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken);
        public Task<long> GetInOrderCopiesAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken);
        public Task<long> GetOrderAmountAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken);
        public Task<Dictionary<DateTime, long>> GetOrderAmountInDaysAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken);
        public Task<long> GetSoldCopiesAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken);
    }
}