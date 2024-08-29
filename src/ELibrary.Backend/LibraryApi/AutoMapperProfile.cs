using AutoMapper;
using LibraryApi.Domain.Dto.Author;
using LibraryApi.Domain.Dto.Book;
using LibraryApi.Domain.Dto.Genre;
using LibraryApi.Domain.Entities;

namespace LibraryApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Author, GetAuthorByIdResponse>();
            CreateMap<Author, CreateAuthorResponse>();
            CreateMap<CreateAuthorRequest, Author>();
            CreateMap<UpdateAuthorRequest, Author>();

            CreateMap<Genre, GetGenreByIdResponse>();
            CreateMap<Genre, CreateGenreResponse>();
            CreateMap<CreateGenreRequest, Genre>();
            CreateMap<UpdateGenreRequest, Genre>();

            CreateMap<Book, GetBookByIdResponse>();
            CreateMap<Book, CreateBookResponse>();
            CreateMap<CreateBookRequest, Book>();
            CreateMap<UpdateBookRequest, Book>();
        }
    }
}