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
            CreateMap<Author, GetAuthorResponse>();
            CreateMap<Author, CreateAuthorResponse>();
            CreateMap<CreateAuthorRequest, Author>();
            CreateMap<UpdateAuthorRequest, Author>();

            CreateMap<Genre, GetGenreResponse>();
            CreateMap<Genre, CreateGenreResponse>();
            CreateMap<CreateGenreRequest, Genre>();
            CreateMap<UpdateGenreRequest, Genre>();

            CreateMap<Book, GetBookResponse>();
            CreateMap<Book, CreateBookResponse>();
            CreateMap<CreateBookRequest, Book>();
            CreateMap<UpdateBookRequest, Book>();
        }
    }
}