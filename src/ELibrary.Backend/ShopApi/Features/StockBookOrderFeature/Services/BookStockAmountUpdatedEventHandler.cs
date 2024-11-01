using EventSourcing;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;
using ShopApi.Features.StockBookOrderFeature.Models;

namespace ShopApi.Features.StockBookOrderFeature.Services
{
    public class BookStockAmountUpdatedEventHandler : IEventHandler<BookStockAmountUpdatedEvent>
    {
        protected readonly IDatabaseRepository<ShopDbContext> repository;

        public BookStockAmountUpdatedEventHandler(IDatabaseRepository<ShopDbContext> repository)
        {
            this.repository = repository;
        }

        public async Task HandleAsync(BookStockAmountUpdatedEvent @event, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);

            foreach (var stockBookChange in @event.StockBookOrder.StockBookChanges)
            {
                var bookInDb = await queryable.FirstAsync(b => b.Id == stockBookChange.BookId, cancellationToken);

                bookInDb.StockAmount += stockBookChange.ChangeAmount;

                await repository.UpdateAsync(bookInDb, cancellationToken);
            }
        }
    }
}
