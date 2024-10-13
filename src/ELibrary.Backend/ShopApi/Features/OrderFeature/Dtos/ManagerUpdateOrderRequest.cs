using LibraryShopEntities.Domain.Entities.Shop;

namespace ShopApi.Features.OrderFeature.Dtos
{
    public class ManagerUpdateOrderRequest
    {
        public int Id { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryTime { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}