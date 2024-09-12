using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Shop;

namespace LibraryShopEntities.Domain.Dtos.Shop
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public int OrderAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryTime { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<BookResponse> Books { get; set; }
    }
}
