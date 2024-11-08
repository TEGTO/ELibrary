using ShopApi.Features.StatisticsFeature.Domain.Models;
using ShopApi.Features.StatisticsFeature.Repository;

namespace ShopApi.Features.StatisticsFeature.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsRepository statisticsRepository;

        public StatisticsService(IStatisticsRepository statisticsRepository)
        {
            this.statisticsRepository = statisticsRepository;
        }

        #region IStatisticsService Members

        public async Task<ShopStatistics> GetStatisticsAsync(GetShopStatisticsFilter getBookStatistics, CancellationToken cancellationToken)
        {
            var inCartCopiesTask = statisticsRepository.GetInCartCopiesAsync(getBookStatistics, cancellationToken);
            var inOrderCopiesTask = statisticsRepository.GetInOrderCopiesAsync(getBookStatistics, cancellationToken);
            var soldCopiesTask = statisticsRepository.GetSoldCopiesAsync(getBookStatistics, cancellationToken);
            var canceledCopiesTask = statisticsRepository.GetCanceledCopiesAsync(getBookStatistics, cancellationToken);
            var orderAmountTask = statisticsRepository.GetOrderAmountAsync(getBookStatistics, cancellationToken);
            var canceledOrderAmountTask = statisticsRepository.GetCanceledOrdersAsync(getBookStatistics, cancellationToken);
            var averagePriceTask = statisticsRepository.GetAveragePriceAsync(getBookStatistics, cancellationToken);
            var earnedMoneyTask = statisticsRepository.GetEarnedMoneyAsync(getBookStatistics, cancellationToken);
            var orderAmountInDaysTask = statisticsRepository.GetOrderAmountInDaysAsync(getBookStatistics, cancellationToken);

            await Task.WhenAll(inCartCopiesTask, inOrderCopiesTask, soldCopiesTask, canceledCopiesTask,
                               orderAmountTask, canceledOrderAmountTask, averagePriceTask, earnedMoneyTask, orderAmountInDaysTask);

            return new ShopStatistics
            {
                InCartCopies = await inCartCopiesTask,
                InOrderCopies = await inOrderCopiesTask,
                SoldCopies = await soldCopiesTask,
                CanceledCopies = await canceledCopiesTask,
                OrderAmount = await orderAmountTask,
                CanceledOrderAmount = await canceledOrderAmountTask,
                AveragePrice = await averagePriceTask,
                EarnedMoney = await earnedMoneyTask,
                OrderAmountInDays = await orderAmountInDaysTask
            };
        }

        #endregion
    }
}
