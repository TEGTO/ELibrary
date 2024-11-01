using MediatR;
using ShopApi.Features.ClientFeature.Services;
using ShopApi.Features.OrderFeature.Services;

namespace ShopApi.Features.OrderFeature.Command.GetOrderAmount
{
    public class GetOrderAmountQueryHandler : IRequestHandler<GetOrderAmountQuery, int>
    {
        private readonly IOrderService orderService;
        private readonly IClientService clientService;

        public GetOrderAmountQueryHandler(
            IOrderService orderService,
            IClientService clientService)
        {
            this.orderService = orderService;
            this.clientService = clientService;
        }

        public async Task<int> Handle(GetOrderAmountQuery command, CancellationToken cancellationToken)
        {
            var client = await clientService.GetClientByUserIdAsync(command.UserId, cancellationToken);

            if (client == null)
            {
                throw new InvalidDataException("Client is not found!");
            }

            command.Request.ClientId = client.Id;
            return await orderService.GetOrderAmountAsync(command.Request, cancellationToken);
        }
    }
}
