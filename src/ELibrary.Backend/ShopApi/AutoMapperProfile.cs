﻿using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using Shared.Domain.Dtos;
using ShopApi.Features.CartFeature.Dtos;
using ShopApi.Features.ClientFeature.Dtos;
using ShopApi.Features.OrderFeature.Dtos;
using ShopApi.Features.StatisticsFeature.Domain.Dtos;
using ShopApi.Features.StatisticsFeature.Domain.Models;
using ShopApi.Features.StockBookOrderFeature.Dtos;

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

            CreateMap<CreateClientRequest, Client>();
            CreateMap<UpdateClientRequest, Client>();
            CreateMap<Client, ClientResponse>();

            CreateMap<Cart, CartResponse>();

            CreateMap<CartBook, BookListingResponse>();
            CreateMap<OrderBook, BookListingResponse>();
            CreateMap<OrderBookRequest, OrderBook>();

            CreateMap<AddBookToCartRequest, CartBook>();
            CreateMap<UpdateCartBookRequest, CartBook>();
            CreateMap<DeleteCartBookFromCartRequest, Book>();
            CreateMap<CartBook, Book>();

            CreateMap<CreateOrderRequest, Order>();
            CreateMap<ManagerUpdateOrderRequest, Order>();
            CreateMap<ClientUpdateOrderRequest, Order>();
            CreateMap<Order, OrderResponse>();

            CreateMap<StockBookOrder, StockBookOrderResponse>();
            CreateMap<CreateStockBookOrderRequest, StockBookOrder>();
            CreateMap<StockBookChangeRequest, StockBookChange>();
            CreateMap<StockBookChange, StockBookChangeResponse>();

            CreateMap<StatisticsBook, Book>();
            CreateMap<GetBookStatisticsRequest, GetBookStatistics>();
            CreateMap<BookStatistics, BookStatisticsResponse>();

            CreateMap<PaginationRequest, GetOrdersFilter>();
        }
    }
}
