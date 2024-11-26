using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.OrderFeature.Services;
using ShopApi.Services;

namespace ShopApi.Features.OrderFeature.Command.ManagerGetOrderById
{
    public class ManagerGetOrderByIdQueryHandler : IRequestHandler<ManagerGetOrderByIdQuery, OrderResponse?>
    {
        private readonly IOrderService orderService;
        private readonly ILibraryService libraryService;
        private readonly IMapper mapper;

        public ManagerGetOrderByIdQueryHandler(
            IOrderService orderService,
            ILibraryService libraryService,
            IMapper mapper)
        {
            this.orderService = orderService;
            this.libraryService = libraryService;
            this.mapper = mapper;
        }

        public async Task<OrderResponse?> Handle(ManagerGetOrderByIdQuery command, CancellationToken cancellationToken)
        {
            var order = await orderService.GetOrderByIdAsync(command.OrderId, cancellationToken);

            if (order == null)
            {
                return null;
            }

            var bookIds = order.OrderBooks.Select(x => x.BookId).Distinct().ToList();
            var bookResponses = await GetLibraryEntityHelper.GetBookResponsesForIdsAsync(bookIds, libraryService, cancellationToken);
            var bookLookup = bookResponses.ToDictionary(book => book.Id);
            var response = mapper.Map<OrderResponse>(order);
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
