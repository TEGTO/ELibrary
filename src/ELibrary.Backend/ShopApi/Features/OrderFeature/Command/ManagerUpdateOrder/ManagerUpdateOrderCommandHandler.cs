using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using MediatR;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.OrderFeature.Command.ManagerUpdateOrder
{
    public class ManagerUpdateOrderCommandHandler : IRequestHandler<ManagerUpdateOrderCommand, OrderResponse>
    {
        private readonly IOrderService orderService;
        private readonly ILibraryService libraryService;
        private readonly IMapper mapper;

        public ManagerUpdateOrderCommandHandler(
            IOrderService orderService,
            ILibraryService libraryService,
            IMapper mapper)
        {
            this.orderService = orderService;
            this.libraryService = libraryService;
            this.mapper = mapper;
        }

        public async Task<OrderResponse> Handle(ManagerUpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = mapper.Map<Order>(command.Request);

            var updatedOrder = await orderService.UpdateOrderAsync(order, cancellationToken);

            return (await GetLibraryEntityHelper.GetOrderResponsesWithBooksAsync([updatedOrder], libraryService, mapper, cancellationToken)).First();
        }
    }
}
