using AutoMapper;
using LibraryApi.Domain.Dto.Author;
using LibraryApi.Domain.Entities;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Route("author")]
    public class AuthorController : BaseLibraryEntityController<Author, GetAuthorResponse, CreateAuthorRequest, CreateAuthorResponse, UpdateAuthorRequest>
    {
        public AuthorController(ILibraryEntityService<Author> entityService, IMapper mapper) : base(entityService, mapper)
        {
        }
    }
}