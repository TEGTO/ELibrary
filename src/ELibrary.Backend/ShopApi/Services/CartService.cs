using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;

namespace ShopApi.Services
{
    public class CartService : ICartService
    {
        private readonly IDatabaseRepository<LibraryShopDbContext> repository;

        public CartService(IDatabaseRepository<LibraryShopDbContext> repository)
        {
            this.repository = repository;
        }

        public async Task<Cart?> GetCartByUserIdAsync(string userId, bool includeProducts, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Cart>(cancellationToken);
            if (includeProducts)
            {
                return await queryable.AsNoTracking()
                    .Include(x => x.CartBooks)
                    .ThenInclude(x => x.Book)
                    .FirstOrDefaultAsync(x => x.UserId == userId);
            }
            else
            {
                return await queryable.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
            }
        }
        public async Task<Cart?> CreateCartAsync(string userId, CancellationToken cancellationToken)
        {
            var cart = new Cart() { UserId = userId };
            return await repository.AddAsync(cart, cancellationToken);
        }
        public async Task<CartBook> AddCartBookAsync(Cart cart, CartBook cartBook, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Cart>(cancellationToken);
            var cartInDb = await queryable.Include(c => c.CartBooks).FirstOrDefaultAsync(x => x.Id == cart.Id);

            if (cartInDb == null)
            {
                throw new InvalidOperationException("Cart not found.");
            }

            var existingCartBook = cartInDb.CartBooks.FirstOrDefault(cb => cb.BookId == cartBook.BookId);

            if (existingCartBook == null)
            {
                cartInDb.CartBooks.Add(cartBook);
                await repository.UpdateAsync(cartInDb, cancellationToken);
            }
            //else
            //{
            //    existingCartBook.BookAmount += cartBook.BookAmount;
            //    await repository.UpdateAsync(existingCartBook, cancellationToken);
            //    return existingCartBook;
            //}

            return cartBook;
        }
        public async Task<CartBook> UpdateCartBookAsync(Cart cart, CartBook cartBook, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<CartBook>(cancellationToken);
            var cartBookInDb = await queryable
                .FirstOrDefaultAsync(x => x.Id == cartBook.Id && x.CartId == cart.Id, cancellationToken);

            if (cartBookInDb == null)
            {
                throw new InvalidOperationException("CartBook not found or does not belong to the specified cart.");
            }

            cartBookInDb.Copy(cartBook);
            return await repository.UpdateAsync(cartBookInDb, cancellationToken);
        }
        public async Task DeleteCartBookAsync(Cart cart, string id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<CartBook>(cancellationToken);
            var cartBookInDb = await queryable
                .FirstOrDefaultAsync(x => x.Id == id && x.CartId == cart.Id, cancellationToken);

            if (cartBookInDb == null)
            {
                throw new InvalidOperationException("CartBook not found or does not belong to the specified cart.");
            }

            await repository.DeleteAsync(cartBookInDb, cancellationToken);
        }
        public async Task<Cart> ClearCartAsync(Cart cart, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Cart>(cancellationToken);
            var cartInDb = await queryable.Include(x => x.CartBooks).FirstAsync(x => x.Id == cart.Id);
            if (cartInDb.CartBooks.Any())
            {
                foreach (var cartBook in cartInDb.CartBooks.ToList())
                {
                    await repository.DeleteAsync(cartBook, cancellationToken);
                }
                cartInDb.CartBooks.Clear();

                await repository.UpdateAsync(cartInDb, cancellationToken);
            }
            return cartInDb;
        }
        public async Task<bool> CheckBookCartAsync(Cart cart, string id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Cart>(cancellationToken);
            return await queryable.AnyAsync(x => x.Id == cart.Id && x.CartBooks.Any(y => y.Id == id));
        }
    }
}
