using AutoMapper;
using LibraryApi.Domain.Dtos;
using LibraryApi.Domain.Dtos.Library.Genre;
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
        LibraryPaginationRequest>
    {
        public GenreController(ILibraryEntityService<Genre> entityService, IMapper mapper) : base(entityService, mapper)
        {
        }
    }
}