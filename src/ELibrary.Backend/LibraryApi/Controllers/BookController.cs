using AutoMapper;
using LibraryApi.Domain.Dto.Book;
using LibraryApi.Services;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.SharedRequests;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    [Route("book")]
    [ApiController]
    public class BookController : BaseLibraryEntityController<
        Book,
        BookResponse,
        CreateBookRequest,
        BookResponse,
        UpdateBookRequest,
        BookResponse,
        BookFilterRequest>
    {
        private readonly IBookService bookService;

        public BookController(
            ILibraryEntityService<Book> entityService,
            IMapper mapper,
            IBookService bookService
            ) : base(entityService, mapper)
        {
            this.bookService = bookService;
        }

        [HttpPost("popularity")]
        [AllowAnonymous]
        public async Task<IActionResult> RaisePopularity(RaiseBookPopularityRequest request, CancellationToken cancellationToken)
        {
            await bookService.RaisePopularityAsync(request.Ids, cancellationToken);
            return Ok();
        }
        [HttpPost("stockamount")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateStockAmount(List<UpdateBookStockAmountRequest> requests, CancellationToken cancellationToken)
        {
            var d = requests.ToDictionary(x => x.BookId, y => y.ChangeAmount);
            await bookService.ChangeBookStockAmount(d, cancellationToken);
            return Ok();
        }
    }
}
