namespace LibraryShopEntities.Domain.Dtos.SharedRequests
{
    public class UpdateBookStockAmountRequest
    {
        public int BookId { get; set; }
        public int ChangeAmount { get; set; }
    }
}
