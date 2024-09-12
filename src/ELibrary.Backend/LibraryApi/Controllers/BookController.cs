using AutoMapper;
using LibraryShopEntities.Domain.Dtos;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Library.Book;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryShopEntities.Controllers
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
