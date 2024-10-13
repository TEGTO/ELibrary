namespace ShopApi.Features.StockBookOrderFeature.Dtos
{
    public class StockBookChangeRequest
    {
        public int BookId { get; set; }
        public int ChangeAmount { get; set; }
    }
}
