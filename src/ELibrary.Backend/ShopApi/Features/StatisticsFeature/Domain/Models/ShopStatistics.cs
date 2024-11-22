namespace ShopApi.Features.StatisticsFeature.Domain.Models
{
    public class ShopStatistics
    {
        public required long InCartCopies { get; set; }
        public required long InOrderCopies { get; set; }
        public required long SoldCopies { get; set; }
        public required long CanceledCopies { get; set; }
        public required long OrderAmount { get; set; }
        public required long CanceledOrderAmount { get; set; }
        public required decimal AveragePrice { get; set; }
        public required decimal EarnedMoney { get; set; }
        public Dictionary<DateTime, long> OrderAmountInDays { get; set; } = new();
    }
}
