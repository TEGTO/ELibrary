using DatabaseControl.Repositories;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;

namespace LibraryShopEntities.Repositories.Shop
{
    public class CartRepository : ICartRepository
    {
        private readonly IDatabaseRepository<ShopDbContext> repository;

        public CartRepository(IDatabaseRepository<ShopDbContext> repository)
        {
            this.repository = repository;
        }

        public async Task<Cart?> GetCartByUserIdAsync(string userId, bool includeBooks, CancellationToken cancellationToken)
        {
            var queryable = await GetQueryableCartAsync(cancellationToken);
            return includeBooks
                ? await queryable.Include(c => c.Books).FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken)
                : await queryable.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
        }
        public async Task<Cart> AddCartAsync(Cart cart, CancellationToken cancellationToken)
        {
            return await repository.AddAsync(cart, cancellationToken);
        }
        public async Task<CartBook?> GetCartBookByIdAsync(string cartId, string id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Cart>(cancellationToken);
            return await queryable.SelectMany(c => c.Books)
                .FirstOrDefaultAsync(cb => cb.CartId == cartId && cb.Id == id, cancellationToken);
        }
        public async Task<CartBook?> GetCartBookByBookIdAsync(string cartId, int bookId, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Cart>(cancellationToken);
            return await queryable.SelectMany(c => c.Books)
                .FirstOrDefaultAsync(cb => cb.CartId == cartId && cb.BookId == bookId, cancellationToken);
        }
        public async Task<Cart> UpdateCartAsync(Cart cart, CancellationToken cancellationToken)
        {
            return await repository.UpdateAsync(cart, cancellationToken);
        }
        public async Task<CartBook> UpdateCartBookAsync(CartBook cartBook, CancellationToken cancellationToken)
        {
            return await repository.UpdateAsync(cartBook, cancellationToken);
        }
        public async Task DeleteCartBookAsync(CartBook cartBook, CancellationToken cancellationToken)
        {
            await repository.DeleteAsync(cartBook, cancellationToken);
        }
        public async Task<bool> CheckBookInCartAsync(string cartId, string id, CancellationToken cancellationToken)
        {
            var queryable = await GetQueryableCartAsync(cancellationToken);
            return await queryable.AnyAsync(c => c.Id == cartId && c.Books.Any(b => b.Id == id), cancellationToken);
        }
        private async Task<IQueryable<Cart>> GetQueryableCartAsync(CancellationToken cancellationToken)
        {
            return await repository.GetQueryableAsync<Cart>(cancellationToken);
        }
    }
}
