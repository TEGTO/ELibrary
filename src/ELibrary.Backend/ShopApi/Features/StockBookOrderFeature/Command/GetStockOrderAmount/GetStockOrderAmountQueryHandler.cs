using MediatR;
using ShopApi.Features.StockBookOrderFeature.Services;

namespace ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderAmount
{
    public class GetStockOrderAmountQueryHandler : IRequestHandler<GetStockOrderAmountQuery, int>
    {
        private readonly IStockBookOrderService stockBookOrderService;

        public GetStockOrderAmountQueryHandler(IStockBookOrderService stockBookOrderService)
        {
            this.stockBookOrderService = stockBookOrderService;
        }

        public async Task<int> Handle(GetStockOrderAmountQuery command, CancellationToken cancellationToken)
        {
            return await stockBookOrderService.GetStockBookAmountAsync(cancellationToken);
        }
    }
}