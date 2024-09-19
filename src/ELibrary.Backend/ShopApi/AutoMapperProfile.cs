using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
using ShopApi.Domain.Dtos.Cart;
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

            CreateMap<CreateClientRequest, Client>();
            CreateMap<UpdateClientRequest, Client>();
            CreateMap<Client, ClientResponse>();

            CreateMap<Cart, CartResponse>();

            CreateMap<CartBook, BookListingResponse>();
            CreateMap<OrderBook, BookListingResponse>();
            CreateMap<OrderBookRequest, OrderBook>();

            CreateMap<AddCartBookToCartRequest, CartBook>();
            CreateMap<UpdateCartBookRequest, CartBook>();

            CreateMap<CreateOrderRequest, Order>();
            CreateMap<ManagerUpdateOrderRequest, Order>();
            CreateMap<ClientUpdateOrderRequest, Order>();
            CreateMap<Order, OrderResponse>();

            CreateMap<CartBook, Book>();
        }
    }
}
