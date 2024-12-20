﻿using Authentication.Models;
using System.Security.Claims;
using UserEntities.Domain.Entities;

namespace UserApi.Services
{
    public interface ITokenService
    {
        public Task<AccessTokenData> CreateNewTokenDataAsync(User user, DateTime refreshTokenExpiryDate, CancellationToken cancellationToken);
        public Task SetRefreshTokenAsync(User user, AccessTokenData accessTokenData, CancellationToken cancellationToken);
        public ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}