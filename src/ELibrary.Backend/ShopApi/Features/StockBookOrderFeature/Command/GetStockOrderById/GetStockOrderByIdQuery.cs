using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;

namespace ShopApi.Features.StockBookOrderFeature.Command.GetStockOrderById
{
    public record GetStockOrderByIdQuery(int StockOrderId) : IRequest<StockBookOrderResponse?>;
}
