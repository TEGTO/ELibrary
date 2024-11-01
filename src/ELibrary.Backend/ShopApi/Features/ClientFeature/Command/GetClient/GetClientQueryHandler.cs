﻿using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using ShopApi.Features.ClientFeature.Dtos;
using ShopApi.Features.ClientFeature.Services;

namespace ShopApi.Features.ClientFeature.Command.GetClient
{
    public class GetClientQueryHandler : IRequestHandler<GetClientQuery, GetClientResponse>
    {
        private readonly IClientService clientService;
        private readonly IMapper mapper;

        public GetClientQueryHandler(IClientService clientService, IMapper mapper)
        {
            this.clientService = clientService;
            this.mapper = mapper;
        }

        public async Task<GetClientResponse> Handle(GetClientQuery command, CancellationToken cancellationToken)
        {
            var client = await clientService.GetClientByUserIdAsync(command.UserId, cancellationToken);

            return new GetClientResponse
            {
                Client = mapper.Map<ClientResponse>(client)
            };
        }
    }
}