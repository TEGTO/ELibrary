using LibraryShopEntities.Domain.Entities.Shop;

namespace ShopApi.Features.CartFeature.Services
{
    public interface ICartService
    {
        public Task<CartBook> AddCartBookAsync(Cart cart, CartBook cartBook, CancellationToken cancellationToken);
        public Task<bool> CheckBookCartAsync(Cart cart, string id, CancellationToken cancellationToken);
        public Task<Cart> DeleteBooksFromCartAsync(Cart cart, int[] bookIds, CancellationToken cancellationToken);
        public Task<Cart> CreateCartAsync(string userId, CancellationToken cancellationToken);
        public Task<Cart?> GetCartByUserIdAsync(string userId, bool includeProducts, CancellationToken cancellationToken);
        public Task<int> GetInCartAmountAsync(Cart cart, CancellationToken cancellationToken);
        public Task<CartBook> UpdateCartBookAsync(Cart cart, CartBook cartBook, CancellationToken cancellationToken);
    }
}