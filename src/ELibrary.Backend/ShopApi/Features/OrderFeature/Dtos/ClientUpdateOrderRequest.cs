namespace ShopApi.Features.OrderFeature.Dtos
{
    public class ClientUpdateOrderRequest
    {
        public int Id { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryTime { get; set; }
    }
}