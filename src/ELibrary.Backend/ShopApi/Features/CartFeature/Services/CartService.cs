using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;

namespace ShopApi.Features.CartFeature.Services
{
    public class CartService : ICartService
    {
        private readonly IDatabaseRepository<ShopDbContext> repository;
        private readonly int maxBookAmount;

        public CartService(IDatabaseRepository<ShopDbContext> repository, IConfiguration configuration)
        {
            this.repository = repository;
            maxBookAmount = int.Parse(configuration[Configuration.SHOP_MAX_ORDER_AMOUNT]!);
        }

        #region ICartService Members

        public async Task<Cart?> GetCartByUserIdAsync(string userId, bool includeBooks, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Cart>(cancellationToken);
            if (includeBooks)
            {
                return await GetQueryableCartWithBook(queryable).FirstOrDefaultAsync(x => x.UserId == userId);
            }
            else
            {
                return await queryable.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
            }
        }
        public async Task<int> GetInCartAmountAsync(Cart cart, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Cart>(cancellationToken);
            return await queryable.Where(x => x.Id == cart.Id).SumAsync(x => x.Books.Sum(y => y.BookAmount));
        }
        public async Task<Cart> CreateCartAsync(string userId, CancellationToken cancellationToken)
        {
            var cart = new Cart() { UserId = userId };
            return await repository.AddAsync(cart, cancellationToken);
        }
        public async Task<CartBook> AddCartBookAsync(Cart cart, CartBook cartBook, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Cart>(cancellationToken);
            var cartInDb = await queryable
                        .Include(x => x.Books)
                        .FirstOrDefaultAsync(x => x.Id == cart.Id);

            if (cartInDb == null)
            {
                throw new InvalidOperationException("Cart is not found.");
            }

            var existingCartBook = cartInDb.Books.Find(cb => cb.BookId == cartBook.BookId);

            if (existingCartBook == null)
            {
                cartInDb.Books.Add(cartBook);
                await repository.UpdateAsync(cartInDb, cancellationToken);
            }
            else if (existingCartBook.BookAmount + cartBook.BookAmount <= maxBookAmount)
            {
                existingCartBook.BookAmount += cartBook.BookAmount;
                await repository.UpdateAsync(existingCartBook, cancellationToken);
            }

            // Refetch the new CartBook including the related entities
            var newCartBook = await GetQueryableCartWithBook(queryable)
                .SelectMany(x => x.Books)
                .FirstOrDefaultAsync(cb => cb.BookId == cartBook.BookId && cb.CartId == cart.Id, cancellationToken);

            return newCartBook!;
        }
        public async Task<CartBook> UpdateCartBookAsync(Cart cart, CartBook cartBook, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<CartBook>(cancellationToken);
            var cartBookInDb = await queryable
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == cartBook.Id && x.CartId == cart.Id, cancellationToken);

            if (cartBookInDb == null)
            {
                throw new InvalidOperationException("CartBook is not found or does not belong to the specified cart.");
            }

            cartBookInDb.Copy(cartBook);
            await repository.UpdateAsync(cartBookInDb, cancellationToken);
            return cartBookInDb;
        }
        public async Task<Cart> DeleteBooksFromCartAsync(Cart cart, int[] bookIds, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Cart>(cancellationToken);
            var cartInDb = await GetQueryableCartWithBook(queryable)
                                          .FirstOrDefaultAsync(x => x.Id == cart.Id, cancellationToken);

            if (cartInDb == null || cartInDb.Books == null || !cartInDb.Books.Any())
            {
                return cartInDb;
            }

            var cartBooksInDB = cartInDb.Books.Where(x => bookIds.Contains(x.BookId)).ToList();

            if (cartBooksInDB.Any())
            {
                foreach (var cartBook in cartBooksInDB)
                {
                    await repository.DeleteAsync(cartBook, cancellationToken);
                    cartInDb.Books.Remove(cartBook);
                }

                await repository.UpdateAsync(cartInDb, cancellationToken);
            }

            return cartInDb;
        }
        public async Task<bool> CheckBookCartAsync(Cart cart, string id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Cart>(cancellationToken);
            return await queryable.AnyAsync(x => x.Id == cart.Id && x.Books.Any(y => y.Id == id));
        }

        #endregion

        #region Private Helpers

        private IQueryable<Cart> GetQueryableCartWithBook(IQueryable<Cart> queryable)
        {
            return queryable
                 .AsNoTracking()
                 .AsSplitQuery()
                     .Include(x => x.Books);
        }

        #endregion
    }
}
