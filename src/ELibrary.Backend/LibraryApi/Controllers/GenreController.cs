using AutoMapper;
using LibraryApi.Domain.Dto.Genre;
using LibraryApi.Domain.Entities;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Route("genre")]
    public class GenreController : BaseLibraryEntityController<Genre, GetGenreByIdResponse, CreateGenreRequest, CreateGenreResponse, UpdateGenreRequest>
    {
        public GenreController(ILibraryEntityService<Genre> entityService, IMapper mapper) : base(entityService, mapper)
        {
        }
    }
}