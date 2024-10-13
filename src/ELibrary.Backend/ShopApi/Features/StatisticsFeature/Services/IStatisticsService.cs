using ShopApi.Features.StatisticsFeature.Domain.Models;

namespace ShopApi.Features.StatisticsFeature.Services
{
    public interface IStatisticsService
    {
        public Task<BookStatistics> GetStatisticsAsync(GetBookStatistics getBookStatistics, CancellationToken cancellationToken);
    }
}