﻿using AutoMapper;
using LibraryApi.Domain.Dto.Book;
using LibraryApi.Domain.Entities;
using LibraryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    [Route("book")]
    [ApiController]
    public class BookController : BaseLibraryEntityController<Book, BookResponse, CreateBookRequest, BookResponse, UpdateBookRequest, BookResponse>
    {
        public BookController(ILibraryEntityService<Book> entityService, IMapper mapper) : base(entityService, mapper)
        {
        }
    }
}
