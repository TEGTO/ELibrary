using LibraryShopEntities.Domain.Entities.Shop;

namespace ShopApi.Features.OrderFeature.Dtos
{
    public class CreateOrderRequest
    {
        public string ContactClientName { get; set; }
        public string ContactPhone { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryTime { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public List<OrderBookRequest> OrderBooks { get; set; }
    }
}
