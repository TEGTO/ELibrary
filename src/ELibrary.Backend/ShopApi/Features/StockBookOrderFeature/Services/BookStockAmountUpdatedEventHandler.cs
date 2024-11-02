using EventSourcing;
using ShopApi.Features.StockBookOrderFeature.Models;
using ShopApi.Services;

namespace ShopApi.Features.StockBookOrderFeature.Services
{
    public class BookStockAmountUpdatedEventHandler : IEventHandler<BookStockAmountUpdatedEvent>
    {
        protected readonly ILibraryService libraryService;

        public BookStockAmountUpdatedEventHandler(ILibraryService libraryService)
        {
            this.libraryService = libraryService;
        }

        public async Task HandleAsync(BookStockAmountUpdatedEvent @event, CancellationToken cancellationToken)
        {
            await libraryService.UpdateBookStockAmountAsync(@event.StockBookOrder.StockBookChanges, cancellationToken);
        }
    }
}
