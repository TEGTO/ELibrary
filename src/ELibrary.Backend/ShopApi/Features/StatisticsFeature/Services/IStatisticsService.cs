using LibraryShopEntities.Filters;
using ShopApi.Features.StatisticsFeature.Domain.Models;

namespace ShopApi.Features.StatisticsFeature.Services
{
    public interface IStatisticsService
    {
        public Task<ShopStatistics> GetStatisticsAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken);
    }
}