using MediatR;

namespace ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderAmount
{
    public record GetStockOrderAmountQuery() : IRequest<int>;
}
