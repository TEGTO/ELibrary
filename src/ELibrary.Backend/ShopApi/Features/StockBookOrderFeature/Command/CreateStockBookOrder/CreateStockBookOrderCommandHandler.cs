using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using MediatR;
using ShopApi.Features.StockBookOrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.StockBookOrderFeature.Command.CreateStockBookOrder
{
    public class CreateStockBookOrderCommandHandler : IRequestHandler<CreateStockBookOrderCommand, StockBookOrderResponse>
    {
        private readonly IStockBookOrderService stockBookOrderService;
        private readonly ILibraryService libraryService;
        private readonly IMapper mapper;

        public CreateStockBookOrderCommandHandler(IStockBookOrderService stockBookOrderService, ILibraryService libraryService, IMapper mapper)
        {
            this.stockBookOrderService = stockBookOrderService;
            this.libraryService = libraryService;
            this.mapper = mapper;
        }

        public async Task<StockBookOrderResponse> Handle(CreateStockBookOrderCommand command, CancellationToken cancellationToken)
        {
            var order = mapper.Map<StockBookOrder>(command.Request);

            var bookIds = command.Request.StockBookChanges.Select(x => x.BookId).Distinct().ToList();
            var bookResponses = await GetLibraryEntityHelper.GetBookResponsesForIdsAsync(bookIds, libraryService, cancellationToken);

            var response = mapper.Map<StockBookOrderResponse>(await stockBookOrderService.AddStockBookOrderAsync(order, cancellationToken));

            var bookLookup = bookResponses.ToDictionary(book => book.Id);
            foreach (var change in response.StockBookChanges)
            {
                if (bookLookup.TryGetValue(change.BookId, out var book))
                {
                    change.Book = book;
                }
            }

            return response;
        }
    }
}
