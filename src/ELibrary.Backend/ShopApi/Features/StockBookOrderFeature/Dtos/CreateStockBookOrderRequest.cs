namespace ShopApi.Features.StockBookOrderFeature.Dtos
{
    public class CreateStockBookOrderRequest
    {
        public string ClientId { get; set; }
        public List<StockBookChangeRequest> StockBookChanges { get; set; }
    }
}
