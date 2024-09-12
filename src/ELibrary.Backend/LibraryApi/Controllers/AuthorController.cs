using AutoMapper;
using LibraryShopEntities.Domain.Dtos;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Library.Author;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryShopEntities.Controllers
{
    [Authorize]
    [Route("author")]
    [ApiController]
    public class AuthorController : BaseLibraryEntityController<
        Author,
        AuthorResponse,
        CreateAuthorRequest,
        AuthorResponse,
        UpdateAuthorRequest,
        AuthorResponse,
        LibraryPaginationRequest>
    {
        public AuthorController(ILibraryEntityService<Author> entityService, IMapper mapper) : base(entityService, mapper)
        {
        }
    }
}