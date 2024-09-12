﻿using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Library.Publisher;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using ShopApi.Domain.Dtos.Client;
using ShopApi.Domain.Dtos.Order;

namespace ShopApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, BookResponse>();
            CreateMap<Author, AuthorResponse>();
            CreateMap<Genre, GenreResponse>();
            CreateMap<Publisher, PublisherResponse>();
            CreateMap<CoverType, CoverTypeResponse>();

            CreateMap<CreateClientRequest, Client>();
            CreateMap<UpdateClientRequest, Client>();
            CreateMap<Client, ClientResponse>();

            CreateMap<CreateOrderRequest, Order>();
            CreateMap<UpdateOrderRequest, Order>();
            CreateMap<Order, OrderResponse>();
        }
    }
}
