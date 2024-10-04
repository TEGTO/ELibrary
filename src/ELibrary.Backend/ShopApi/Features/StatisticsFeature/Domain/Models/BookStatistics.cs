namespace ShopApi.Features.StatisticsFeature.Domain.Models
{
    public class BookStatistics
    {
        public int InCartCopies { get; set; }
        public int InOrderCopies { get; set; }
        public int SoldCopies { get; set; }
        public int CanceledOrders { get; set; }
        public decimal AveragePrice { get; set; }
        public int StockAmount { get; set; }
        public decimal EarnedMoney { get; set; }
    }
}
