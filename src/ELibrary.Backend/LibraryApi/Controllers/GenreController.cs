using AutoMapper;
using LibraryApi.Domain.Dto.Genre;
using LibraryApi.Domain.Entities;
using LibraryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    [Route("genre")]
    [ApiController]
    public class GenreController : BaseLibraryEntityController<Genre, GenreResponse, CreateGenreRequest, GenreResponse, UpdateGenreRequest>
    {
        public GenreController(ILibraryEntityService<Genre> entityService, IMapper mapper) : base(entityService, mapper)
        {
        }
    }
}