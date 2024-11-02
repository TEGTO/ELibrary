using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using MediatR;
using ShopApi.Features.ClientFeature.Services;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Features.StockBookOrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.OrderFeature.Command.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
    {
        private readonly IOrderService orderService;
        private readonly IClientService clientService;
        private readonly ILibraryService libraryService;
        private readonly IStockBookOrderService stockBookOrderService;
        private readonly IMapper mapper;

        public CreateOrderCommandHandler(
            IOrderService orderService,
            IClientService clientService,
            IStockBookOrderService stockBookOrderService,
            ILibraryService libraryService,
            IMapper mapper)
        {
            this.orderService = orderService;
            this.clientService = clientService;
            this.libraryService = libraryService;
            this.stockBookOrderService = stockBookOrderService;
            this.mapper = mapper;
        }

        public async Task<OrderResponse> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var client = await clientService.GetClientByUserIdAsync(command.UserId, cancellationToken);

            if (client == null)
            {
                throw new InvalidDataException("Client is not found!");
            }

            var order = mapper.Map<Order>(command.Request);
            order.ClientId = client.Id;

            var bookIds = command.Request.OrderBooks.Select(x => x.BookId).Distinct().ToList();
            var bookResponses = await GetLibraryEntityHelper.GetBookResponsesForIdsAsync(bookIds, libraryService, cancellationToken);
            var bookLookup = bookResponses.ToDictionary(book => book.Id);

            foreach (var orderBook in order.OrderBooks)
            {
                if (bookLookup.TryGetValue(orderBook.BookId, out var book))
                {
                    orderBook.BookPrice = book.Price;
                }
            }

            await libraryService.RaiseBookPopularityByIdsAsync(bookIds, cancellationToken);

            await stockBookOrderService.AddStockBookOrderAsyncFromOrderAsync(order, StockBookOrderType.ClientOrder, cancellationToken);
            var createdOrder = await orderService.CreateOrderAsync(order, cancellationToken);

            var response = mapper.Map<OrderResponse>(createdOrder);
            foreach (var listingBook in response.OrderBooks)
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
