using AutoMapper;
using LibraryShopEntities.Domain.Dtos;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Library.Genre;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryShopEntities.Controllers
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
        LibraryPaginationRequest>
    {
        public GenreController(ILibraryEntityService<Genre> entityService, IMapper mapper) : base(entityService, mapper)
        {
        }
    }
}