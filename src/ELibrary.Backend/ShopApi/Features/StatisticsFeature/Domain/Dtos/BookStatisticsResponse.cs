namespace ShopApi.Features.StatisticsFeature.Domain.Dtos
{
    public class BookStatisticsResponse
    {
        public int InOrderCopies { get; set; }
        public int InCartCopies { get; set; }
        public int SoldCopies { get; set; }
        public int CanceledOrders { get; set; }
        public float AveragePrice { get; set; }
        public int StockAmount { get; set; }
        public decimal EarnedMoney { get; set; }
    }
}