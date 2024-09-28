using LibraryShopEntities.Domain.Entities.Shop;

namespace ShopApi.Features.StockBookOrderFeature.Dtos
{
    public class CreateStockBookOrderRequest
    {
        public StockBookOrderType Type { get; set; }
        public string ClientId { get; set; }
        public List<StockBookChangeRequest> StockBookChanges { get; set; }
    }
}
