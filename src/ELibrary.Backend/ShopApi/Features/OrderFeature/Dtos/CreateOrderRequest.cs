using LibraryShopEntities.Domain.Entities.Shop;

namespace ShopApi.Features.OrderFeature.Dtos
{
    public class CreateOrderRequest
    {
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryTime { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<OrderBookRequest> OrderBooks { get; set; }
    }
}
