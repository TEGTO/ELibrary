using AutoMapper;
using Caching.Helpers;
using Caching.Services;
using LibraryApi.Domain.Dto.Genre;
using LibraryApi.Domain.Dtos;
using LibraryApi.Services;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    [Route("genre")]
    [ApiController]
    public class GenreController : BaseLibraryEntityController<
        Genre,
        GenreResponse,
        CreateGenreRequest,
        GenreResponse,
        UpdateGenreRequest,
        GenreResponse,
        LibraryFilterRequest>
    {
        public GenreController(
            ILibraryEntityService<Genre> entityService,
            ICacheService cacheService,
            ICachingHelper cachingHelper,
            IMapper mapper
            ) : base(entityService, cacheService, cachingHelper, mapper)
        {
        }
    }
}