using ShopApi.Features.StatisticsFeature.Domain.Models;

namespace ShopApi.Features.StatisticsFeature.Services
{
    public interface IStatisticsService
    {
        public Task<ShopStatistics> GetStatisticsAsync(GetShopStatistics getBookStatistics, CancellationToken cancellationToken);
    }
}