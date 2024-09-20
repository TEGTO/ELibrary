namespace ShopApi.Domain.Dtos.Cart
{
    public class AddBookToCartRequest
    {
        public int BookAmount { get; set; }
        public int BookId { get; set; }
    }
}
