using LibraryShopEntities.Domain.Entities.Shop;

namespace ShopApi.Features.StockBookOrderFeature.Models
{
    public class BookStockAmountUpdatedEvent
    {
        public required StockBookOrder StockBookOrder { get; set; }
    }
}
