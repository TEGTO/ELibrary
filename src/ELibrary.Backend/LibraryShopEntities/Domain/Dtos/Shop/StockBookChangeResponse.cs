using LibraryShopEntities.Domain.Dtos.Library;

namespace LibraryShopEntities.Domain.Dtos.Shop
{
    public class StockBookChangeResponse
    {
        public int Id { get; set; }
        public BookResponse Book { get; set; } = default!;
        public int ChangeAmount { get; set; }
    }
}
