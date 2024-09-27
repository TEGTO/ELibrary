namespace LibraryShopEntities.Domain.Dtos.Shop
{
    public class StockBookOrderResponse
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int TotalChangeAmount { get; set; }
        public ClientResponse Client { get; set; }
        public List<StockBookChangeResponse> StockBookChanges { get; set; }
    }
}
