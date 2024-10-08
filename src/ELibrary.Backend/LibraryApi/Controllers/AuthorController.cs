﻿using AutoMapper;
using LibraryApi.Domain.Dto.Author;
using LibraryApi.Domain.Entities;
using LibraryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    [Route("author")]
    [ApiController]
    public class AuthorController : BaseLibraryEntityController<Author, AuthorResponse, CreateAuthorRequest, AuthorResponse, UpdateAuthorRequest, AuthorResponse>
    {
        public AuthorController(ILibraryEntityService<Author> entityService, IMapper mapper) : base(entityService, mapper)
        {
        }
    }
}