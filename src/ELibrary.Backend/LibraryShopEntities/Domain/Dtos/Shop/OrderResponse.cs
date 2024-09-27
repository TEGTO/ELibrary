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
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryTime { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<BookListingResponse> OrderBooks { get; set; }
    }
}
