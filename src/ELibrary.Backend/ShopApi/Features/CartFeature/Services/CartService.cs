using LibraryShopEntities.Domain.Entities.Shop;
using LibraryShopEntities.Repositories.Shop;
using Microsoft.EntityFrameworkCore;

namespace ShopApi.Features.CartFeature.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository repository;
        private readonly int maxBookAmount;

        public CartService(ICartRepository repository, IConfiguration configuration)
        {
            this.repository = repository;
            maxBookAmount = int.Parse(configuration[Configuration.SHOP_MAX_ORDER_AMOUNT]!);
        }

        #region ICartService Members

        public async Task<Cart?> GetCartByUserIdAsync(string userId, bool includeBooks, CancellationToken cancellationToken)
        {
            return await repository.GetCartByUserIdAsync(userId, includeBooks, cancellationToken);
        }
        public async Task<int> GetInCartAmountAsync(Cart cart, CancellationToken cancellationToken)
        {
            var cartInDb = await repository.GetCartByUserIdAsync(cart.UserId, includeBooks: true, cancellationToken);
            return cartInDb?.Books.Sum(b => b.BookAmount) ?? 0;
        }
        public async Task<Cart> CreateCartAsync(string userId, CancellationToken cancellationToken)
        {
            var cart = new Cart { UserId = userId };
            return await repository.AddCartAsync(cart, cancellationToken);
        }
        public async Task<CartBook> AddCartBookAsync(Cart cart, CartBook cartBook, CancellationToken cancellationToken)
        {
            var cartInDb = await repository.GetCartByUserIdAsync(cart.UserId, includeBooks: true, cancellationToken);
            if (cartInDb == null) throw new InvalidOperationException("Cart is not found.");

            var existingCartBook = cartInDb.Books.Find(cb => cb.BookId == cartBook.BookId);

            if (existingCartBook == null)
            {
                cartInDb.Books.Add(cartBook);
                await repository.UpdateCartAsync(cartInDb, cancellationToken);
            }
            else if (existingCartBook.BookAmount + cartBook.BookAmount <= maxBookAmount)
            {
                existingCartBook.BookAmount += cartBook.BookAmount;
                await repository.UpdateCartBookAsync(existingCartBook, cancellationToken);
            }

            return await repository.GetCartBookByBookIdAsync(cart.Id, cartBook.BookId, cancellationToken)
            ?? throw new InvalidOperationException("Failed to add or update cart book.");
        }
        public async Task<CartBook> UpdateCartBookAsync(Cart cart, CartBook cartBook, CancellationToken cancellationToken)
        {
            var cartBookInDb = await repository.GetCartBookByIdAsync(cart.Id, cartBook.Id, cancellationToken);
            if (cartBookInDb == null) throw new InvalidOperationException("CartBook is not found or does not belong to the specified cart.");

            cartBookInDb.Copy(cartBook);
            return await repository.UpdateCartBookAsync(cartBookInDb, cancellationToken);
        }
        public async Task<Cart> DeleteBooksFromCartAsync(Cart cart, int[] bookIds, CancellationToken cancellationToken)
        {
            var cartInDb = await repository.GetCartByUserIdAsync(cart.UserId, includeBooks: true, cancellationToken);
            if (cartInDb == null || cartInDb.Books == null || !cartInDb.Books.Any()) return cartInDb;

            var cartBooksToDelete = cartInDb.Books.Where(b => bookIds.Contains(b.BookId)).ToList();
            if (cartBooksToDelete.Any())
            {
                foreach (var cartBook in cartBooksToDelete)
                {
                    await repository.DeleteCartBookAsync(cartBook, cancellationToken);
                    cartInDb.Books.Remove(cartBook);
                }
            }

            await repository.UpdateCartAsync(cartInDb, cancellationToken);
            return cartInDb;
        }
        public async Task<bool> CheckBookCartAsync(Cart cart, string id, CancellationToken cancellationToken)
        {
            return await repository.CheckBookInCartAsync(cart.Id, id, cancellationToken);
        }

        #endregion
    }
}
