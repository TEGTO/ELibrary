using AutoMapper;
using LibraryApi.Domain.Dto.Genre;
using LibraryApi.Services;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;
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
            IMapper mapper
            ) : base(entityService, mapper)
        {
        }
    }
}