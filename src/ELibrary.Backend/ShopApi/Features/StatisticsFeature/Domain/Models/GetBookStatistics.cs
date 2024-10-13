using LibraryShopEntities.Domain.Entities.Library;

namespace ShopApi.Features.StatisticsFeature.Domain.Models
{
    public class GetBookStatistics
    {
        public DateTime? FromUTC { get; set; }
        public DateTime? ToUTC { get; set; }
        public Book[] IncludeBooks { get; set; }
    }
}
