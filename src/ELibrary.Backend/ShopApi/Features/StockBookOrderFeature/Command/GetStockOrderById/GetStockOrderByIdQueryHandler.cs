using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.StockBookOrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderById
{
    public class GetStockOrderByIdQueryHandler : IRequestHandler<GetStockOrderByIdQuery, StockBookOrderResponse?>
    {
        private readonly IStockBookOrderService stockBookOrderService;
        private readonly ILibraryService libraryService;
        private readonly IMapper mapper;

        public GetStockOrderByIdQueryHandler(IStockBookOrderService stockBookOrderService, ILibraryService libraryService, IMapper mapper)
        {
            this.stockBookOrderService = stockBookOrderService;
            this.libraryService = libraryService;
            this.mapper = mapper;
        }

        public async Task<StockBookOrderResponse?> Handle(GetStockOrderByIdQuery command, CancellationToken cancellationToken)
        {
            var order = await stockBookOrderService.GetStockBookOrderByIdAsync(command.StockOrderId, cancellationToken);

            var response = mapper.Map<StockBookOrderResponse>(order);

            var bookIds = response.StockBookChanges.Select(x => x.BookId).Distinct().ToList();
            var bookResponses = await GetLibraryEntityHelper.GetBookResponsesForIdsAsync(bookIds, libraryService, cancellationToken);
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
