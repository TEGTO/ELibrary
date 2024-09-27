using LibraryShopEntities.Domain.Entities.Shop;

namespace ShopApi.Features.StockBookOrderFeature.Models
{
    public class BookStockAmountUpdatedEvent
    {
        public StockBookOrder StockBookOrder { get; }

        public BookStockAmountUpdatedEvent(StockBookOrder stockBookOrder)
        {
            StockBookOrder = stockBookOrder;
        }
    }
}
