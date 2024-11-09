using LibraryShopEntities.Domain.Entities.Shop;

namespace LibraryShopEntities.Repositories.Shop
{
    public interface ICartRepository
    {
        public Task<Cart> AddCartAsync(Cart cart, CancellationToken cancellationToken);
        public Task<bool> CheckBookInCartAsync(string cartId, string id, CancellationToken cancellationToken);
        public Task DeleteCartBookAsync(CartBook cartBook, CancellationToken cancellationToken);
        public Task<CartBook?> GetCartBookByIdAsync(string cartId, string id, CancellationToken cancellationToken);
        public Task<Cart?> GetCartByUserIdAsync(string userId, bool includeBooks, CancellationToken cancellationToken);
        public Task<CartBook?> GetCartBookByBookIdAsync(string cartId, int bookId, CancellationToken cancellationToken);
        public Task<Cart> UpdateCartAsync(Cart cart, CancellationToken cancellationToken);
        public Task<CartBook> UpdateCartBookAsync(CartBook cartBook, CancellationToken cancellationToken);
    }
}