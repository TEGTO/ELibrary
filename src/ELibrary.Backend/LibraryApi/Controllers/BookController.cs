using AutoMapper;
using LibraryApi.Domain.Dto.Book;
using LibraryApi.Domain.Dtos;
using LibraryApi.Services;
using LibraryShopEntities.Domain.Entities.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryShopEntities.Domain.Dtos.Library;

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

        public BookController(ILibraryEntityService<Book> entityService, IMapper mapper, IBookService bookService) : base(entityService, mapper)
        {
            this.bookService = bookService;
        }

        [HttpPost("popularity")]
        [AllowAnonymous]
        public virtual async Task<IActionResult> RaisePopularity(List<int> ids, CancellationToken cancellationToken)
        {
            await bookService.RaisePopularityAsync(ids, cancellationToken);
            return Ok();
        }
    }
}
