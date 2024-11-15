using LibraryShopEntities.Domain.Entities.Shop;

namespace ShopApi.Features.OrderFeature.Dtos
{
    public class ClientUpdateOrderRequest
    {
        public int Id { get; set; }
        public string ContactClientName { get; set; }
        public string ContactPhone { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryTime { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
    }
}