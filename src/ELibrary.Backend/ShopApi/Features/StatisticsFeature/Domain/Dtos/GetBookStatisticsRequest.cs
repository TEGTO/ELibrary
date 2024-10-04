namespace ShopApi.Features.StatisticsFeature.Domain.Dtos
{
    public class GetBookStatisticsRequest
    {
        public DateTime? FromUTC { get; set; }
        public DateTime? ToUTC { get; set; }
        public StatisticsBook[] IncludeBooks { get; set; }
    }
}
