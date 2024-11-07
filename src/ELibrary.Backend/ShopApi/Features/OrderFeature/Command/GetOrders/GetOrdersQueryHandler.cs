using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.ClientFeature.Services;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.OrderFeature.Command.GetOrders
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderResponse>>
    {
        private readonly IOrderService orderService;
        private readonly IClientService clientService;
        private readonly ILibraryService libraryService;
        private readonly IMapper mapper;

        public GetOrdersQueryHandler(
            IOrderService orderService,
            IClientService clientService,
            ILibraryService libraryService,
            IMapper mapper)
        {
            this.orderService = orderService;
            this.clientService = clientService;
            this.libraryService = libraryService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<OrderResponse>> Handle(GetOrdersQuery command, CancellationToken cancellationToken)
        {
            var client = await clientService.GetClientByUserIdAsync(command.UserId, cancellationToken);

            if (client == null)
            {
                throw new InvalidDataException("Client is not found!");
            }

            command.Request.ClientId = client.Id;
            var orders = await orderService.GetPaginatedOrdersAsync(command.Request, cancellationToken);
            return await GetLibraryEntityHelper.GetOrderResponsesWithBooksAsync(orders, libraryService, mapper, cancellationToken);
        }
    }
}