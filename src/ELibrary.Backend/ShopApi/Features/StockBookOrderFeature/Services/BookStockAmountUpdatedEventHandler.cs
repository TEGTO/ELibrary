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
        protected readonly IDatabaseRepository<LibraryShopDbContext> repository;

        public BookStockAmountUpdatedEventHandler(IDatabaseRepository<LibraryShopDbContext> repository)
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

                if (bookInDb.StockAmount < 0)
                {
                    throw new InvalidDataException("Invalid book stock amount, must be greater or equal to 0!");
                }

                await repository.UpdateAsync(bookInDb, cancellationToken);
            }
        }
    }
}
