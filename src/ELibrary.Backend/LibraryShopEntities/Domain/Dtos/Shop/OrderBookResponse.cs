using LibraryShopEntities.Domain.Dtos.Library;

namespace LibraryShopEntities.Domain.Dtos.Shop
{
    public class OrderBookResponse
    {
        public string? Id { get; set; }
        public int BookAmount { get; set; }
        public int BookId { get; set; }
        public decimal BookPrice { get; set; }
        public BookResponse? Book { get; set; }
    }
}
