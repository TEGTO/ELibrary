using LibraryShopEntities.Domain.Dtos.Library;

namespace LibraryShopEntities.Domain.Dtos.Shop
{
    public class StockBookChangeResponse
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public BookResponse? Book { get; set; }
        public int ChangeAmount { get; set; }
    }
}
