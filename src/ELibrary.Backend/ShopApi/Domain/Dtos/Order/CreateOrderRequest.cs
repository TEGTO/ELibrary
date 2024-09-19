namespace ShopApi.Domain.Dtos.Order
{
    public class CreateOrderRequest
    {
        public string DeliveryAddress { get; set; }
        public DateTime DeliveryTime { get; set; }
        public List<OrderBookRequest> OrderBooks { get; set; }
    }
}
