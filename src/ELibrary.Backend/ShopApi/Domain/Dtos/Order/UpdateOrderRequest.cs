using LibraryShopEntities.Domain.Entities.Shop;

namespace ShopApi.Domain.Dtos.Order
{
    public class UpdateOrderRequest
    {
        public int Id { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryTime { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<OrderBook> Books { get; set; }
    }
}