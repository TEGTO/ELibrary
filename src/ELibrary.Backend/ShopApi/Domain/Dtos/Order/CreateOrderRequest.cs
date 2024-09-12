using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Shop;

namespace ShopApi.Domain.Dtos.Order
{
    public class CreateOrderRequest
    {
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryTime { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<BookResponse> Books { get; set; }
    }
}
