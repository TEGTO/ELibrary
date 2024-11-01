using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.OrderFeature.Command.ManagerGetPaginatedOrders
{
    public class ManagerGetPaginatedOrdersQueryHandler : IRequestHandler<ManagerGetPaginatedOrdersQuery, IEnumerable<OrderResponse>>
    {
        private readonly IOrderService orderService;
        private readonly ILibraryService libraryService;
        private readonly IMapper mapper;

        public ManagerGetPaginatedOrdersQueryHandler(
            IOrderService orderService,
            ILibraryService libraryService,
            IMapper mapper)
        {
            this.orderService = orderService;
            this.libraryService = libraryService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<OrderResponse>> Handle(ManagerGetPaginatedOrdersQuery command, CancellationToken cancellationToken)
        {
            var orders = await orderService.GetPaginatedOrdersAsync(command.Filter, cancellationToken);
            return await GetLibraryEntityHelper.GetOrderResponsesWiithBooksAsync(orders, libraryService, mapper, cancellationToken);
        }
    }
}
