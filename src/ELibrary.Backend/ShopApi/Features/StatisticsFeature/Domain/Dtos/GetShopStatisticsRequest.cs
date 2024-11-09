namespace ShopApi.Features.StatisticsFeature.Domain.Dtos
{
    public class GetShopStatisticsRequest
    {
        public DateTime? FromUTC { get; set; }
        public DateTime? ToUTC { get; set; }
        public StatisticsBookRequest[] IncludeBooks { get; set; }
    }
}
