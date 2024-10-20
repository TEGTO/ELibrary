﻿using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using ShopApi.Features.OrderFeature.Dtos;

namespace ShopApi.Features.OrderFeature.Services
{
    public interface IOrderManager
    {
        public Task<OrderResponse> CancelOrderAsync(int orderId, CancellationToken cancellationToken);
        public Task<OrderResponse> CancelOrderAsync(int orderId, Client client, CancellationToken cancellationToken);
        public Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request, Client client, CancellationToken cancellationToken);
        public Task<IEnumerable<OrderResponse>> GetPaginatedOrdersAsync(GetOrdersFilter filter, CancellationToken cancellationToken);
        public Task<IEnumerable<OrderResponse>> GetPaginatedOrdersAsync(GetOrdersFilter filter, Client client, CancellationToken cancellationToken);
        public Task<OrderResponse?> GetOrderByIdAsync(int id, CancellationToken cancellationToken);
        public Task<int> GetOrderAmountAsync(GetOrdersFilter filter, CancellationToken cancellationToken);
        public Task<int> GetOrderAmountAsync(GetOrdersFilter filter, Client client, CancellationToken cancellationToken);
        public Task<OrderResponse> UpdateOrderAsync(ClientUpdateOrderRequest request, Client client, CancellationToken cancellationToken);
        public Task<OrderResponse> UpdateOrderAsync(ManagerUpdateOrderRequest request, CancellationToken cancellationToken);
    }
}