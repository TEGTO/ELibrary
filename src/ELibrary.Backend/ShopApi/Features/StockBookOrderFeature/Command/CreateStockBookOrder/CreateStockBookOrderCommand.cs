using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.StockBookOrderFeature.Dtos;

namespace ShopApi.Features.StockBookOrderFeature.Command.CreateStockBookOrder
{
    public record CreateStockBookOrderCommand(CreateStockBookOrderRequest Request) : IRequest<StockBookOrderResponse>;
}
