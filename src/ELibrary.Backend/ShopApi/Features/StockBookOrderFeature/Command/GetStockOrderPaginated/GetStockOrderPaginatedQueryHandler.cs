using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.StockBookOrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderPaginated
{
    public class GetStockOrderPaginatedQueryHandler : IRequestHandler<GetStockOrderPaginatedQuery, IEnumerable<StockBookOrderResponse>>
    {
        private readonly IStockBookOrderService stockBookOrderService;
        private readonly ILibraryService libraryService;
        private readonly IMapper mapper;

        public GetStockOrderPaginatedQueryHandler(IStockBookOrderService stockBookOrderService, ILibraryService libraryService, IMapper mapper)
        {
            this.stockBookOrderService = stockBookOrderService;
            this.libraryService = libraryService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<StockBookOrderResponse>> Handle(GetStockOrderPaginatedQuery command, CancellationToken cancellationToken)
        {
            var orders = await stockBookOrderService.GetPaginatedStockBookOrdersAsync(command.PaginationRequest, cancellationToken);

            return await GetLibraryEntityHelper.GetStockBookOrderResponseWiithBooksAsync(orders, libraryService, mapper, cancellationToken);
        }
    }
}
