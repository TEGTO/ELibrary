using AutoMapper;
using LibraryApi.Domain.Dtos;
using LibraryApi.Domain.Dtos.Library.Book;
using LibraryApi.Services;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;
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
        BookPaginationRequest>
    {
        public BookController(ILibraryEntityService<Book> entityService, IMapper mapper) : base(entityService, mapper)
        {
        }
    }
}
