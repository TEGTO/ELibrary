﻿using AutoMapper;
using LibraryApi.Domain.Dto.Author;
using LibraryApi.Domain.Dto.Book;
using LibraryApi.Domain.Dto.Genre;
using LibraryApi.Domain.Dto.Publisher;
using LibraryShopEntities.Domain.Dtos.Library;
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
        }
    }
}