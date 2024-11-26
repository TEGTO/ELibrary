using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.ClientFeature.Services;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.OrderFeature.Command.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderResponse>
    {
        private readonly IOrderService orderService;
        private readonly IClientService clientService;
        private readonly ILibraryService libraryService;
        private readonly IMapper mapper;

        public UpdateOrderCommandHandler(
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

        public async Task<OrderResponse> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var client = await clientService.GetClientByUserIdAsync(command.UserId, cancellationToken);

            if (client == null)
            {
                throw new InvalidDataException("Client is not found!");
            }

            var order = await orderService.GetOrderByIdAsync(command.Request.Id, cancellationToken);

            if (order == null || order.ClientId != client.Id)
            {
                throw new InvalidOperationException("Order is not found.");
            }

            var bookIds = order.OrderBooks.Select(x => x.BookId).Distinct().ToList();
            var bookResponses = await GetLibraryEntityHelper.GetBookResponsesForIdsAsync(bookIds, libraryService, cancellationToken);

            mapper.Map(command.Request, order);
            var updatedOrder = await orderService.UpdateOrderAsync(order, cancellationToken);

            var bookLookup = bookResponses.ToDictionary(book => book.Id);
            var response = mapper.Map<OrderResponse>(updatedOrder);
            foreach (var listingBook in response.OrderBooks ?? [])
            {
                if (bookLookup.TryGetValue(listingBook.BookId, out var book))
                {
                    listingBook.Book = book;
                }
            }

            return response;
        }
    }
}