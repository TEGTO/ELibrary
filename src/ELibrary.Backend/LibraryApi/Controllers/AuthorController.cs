using AutoMapper;
using Caching.Helpers;
using Caching.Services;
using LibraryApi.Domain.Dto.Author;
using LibraryApi.Domain.Dtos;
using LibraryApi.Services;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;
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
            ICacheService cacheService,
            ICachingHelper cachingHelper,
            IMapper mapper
            ) : base(entityService, cacheService, cachingHelper, mapper)
        {
        }
    }
}