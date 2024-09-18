namespace ShopApi.Domain.Dtos.Order
{
    public class PatchOrderRequest
    {
        public int Id { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryTime { get; set; }
    }
}