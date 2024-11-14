using AutoMapper;
using LibraryApi.Domain.Dto.Author;
using LibraryApi.Services;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
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
        LibraryFilterRequest>
    {
        public AuthorController(
            ILibraryEntityService<Author> entityService,
            IMapper mapper
            ) : base(entityService, mapper)
        {
        }
    }
}