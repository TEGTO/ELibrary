using LibraryShopEntities.Domain.Entities.Shop;

namespace LibraryShopEntities.Domain.Dtos.Shop
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int OrderAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public string ContactClientName { get; set; } = default!;
        public string ContactPhone { get; set; } = default!;
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryTime { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public ClientResponse Client { get; set; }
        public List<OrderBookResponse> OrderBooks { get; set; }
    }
}
