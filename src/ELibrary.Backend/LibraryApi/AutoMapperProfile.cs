using AutoMapper;
using LibraryApi.Domain.Dtos.Library.Author;
using LibraryApi.Domain.Dtos.Library.Book;
using LibraryApi.Domain.Dtos.Library.CoverType;
using LibraryApi.Domain.Dtos.Library.Genre;
using LibraryApi.Domain.Dtos.Library.Publisher;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Library.Publisher;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, BookResponse>();
            CreateMap<CreateBookRequest, Book>();
            CreateMap<UpdateBookRequest, Book>();

            CreateMap<Author, AuthorResponse>();
            CreateMap<CreateAuthorRequest, Author>();
            CreateMap<UpdateAuthorRequest, Author>();

            CreateMap<Genre, GenreResponse>();
            CreateMap<CreateGenreRequest, Genre>();
            CreateMap<UpdateGenreRequest, Genre>();

            CreateMap<Publisher, PublisherResponse>();
            CreateMap<CreatePublisherRequest, Publisher>();
            CreateMap<UpdatePublisherRequest, Publisher>();

            CreateMap<CoverType, CoverTypeResponse>();
            CreateMap<CreateCoverTypeRequest, CoverType>();
            CreateMap<UpdateCoverTypeRequest, CoverType>();
        }
    }
}