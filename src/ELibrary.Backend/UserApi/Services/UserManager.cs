﻿using Authentication.Identity;
using Authentication.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Shared.Exceptions;
using System.Security.Claims;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;
using UserApi.Domain.Models;
using UserEntities.Domain.Entities;

namespace UserApi.Services
{
    public class UserManager : IUserManager
    {
        private readonly IMapper mapper;
        private readonly IAuthService authService;
        private readonly double expiryInDays;

        public UserManager(IMapper mapper, IAuthService authService, IConfiguration configuration)
        {
            this.mapper = mapper;
            this.authService = authService;
            expiryInDays = double.Parse(configuration[Configuration.AUTH_REFRESH_TOKEN_EXPIRY_IN_DAYS]!);
        }

        public async Task<UserAuthenticationResponse> RegisterUserAsync(UserRegistrationRequest request)
        {
            var user = mapper.Map<User>(request);
            var errors = new List<IdentityError>();

            var registerParams = new RegisterUserParams(user, request.Password);
            errors.AddRange((await authService.RegisterUserAsync(registerParams)).Errors);
            if (Utilities.HasErrors(errors, out var errorResponse)) throw new AuthorizationException(errorResponse);

            errors.AddRange(await authService.SetUserRolesAsync(user, new() { Roles.CLIENT }));
            if (Utilities.HasErrors(errors, out errorResponse)) throw new AuthorizationException(errorResponse);

            var loginParams = new LoginUserParams(request.Email, request.Password, expiryInDays);
            var token = await authService.LoginUserAsync(loginParams);

            var tokenDto = mapper.Map<AuthToken>(token);
            var roles = await authService.GetUserRolesAsync(user);

            return new UserAuthenticationResponse
            {
                AuthToken = tokenDto,
                Email = user.Email,
                Roles = roles
            };
        }
        public async Task<UserAuthenticationResponse> LoginUserAsync(UserAuthenticationRequest request)
        {
            var user = await authService.GetUserByLoginAsync(request.Login);
            if (user == null) throw new UnauthorizedAccessException();

            var loginParams = new LoginUserParams(request.Login, request.Password, expiryInDays);
            var token = await authService.LoginUserAsync(loginParams);

            var tokenDto = mapper.Map<AuthToken>(token);
            var roles = await authService.GetUserRolesAsync(user);

            return new UserAuthenticationResponse
            {
                AuthToken = tokenDto,
                Email = user.Email,
                Roles = roles
            };
        }
        public async Task UpdateUserAsync(UserUpdateDataRequest request, ClaimsPrincipal userClaims, CancellationToken cancellationToken)
        {
            var updateData = mapper.Map<UserUpdateData>(request);
            var user = await authService.GetUserAsync(userClaims);

            var identityErrors = await authService.UpdateUserAsync(user, updateData, false);
            if (Utilities.HasErrors(identityErrors, out var errorResponse)) throw new AuthorizationException(errorResponse);
        }
        public async Task<AuthToken> RefreshTokenAsync(AuthToken request)
        {
            var tokenData = mapper.Map<AccessTokenData>(request);
            var newToken = await authService.RefreshTokenAsync(tokenData, expiryInDays);
            return mapper.Map<AuthToken>(newToken);
        }

        #region Admin Operations

        public async Task<AdminUserResponse> AdminRegisterUserAsync(AdminUserRegistrationRequest request)
        {
            var user = mapper.Map<User>(request);
            var errors = new List<IdentityError>();

            var registerParams = new RegisterUserParams(user, request.Password);
            errors.AddRange((await authService.RegisterUserAsync(registerParams)).Errors);
            if (Utilities.HasErrors(errors, out var errorResponse)) throw new AuthorizationException(errorResponse);

            errors.AddRange(await authService.SetUserRolesAsync(user, request.Roles));
            if (Utilities.HasErrors(errors, out errorResponse)) throw new AuthorizationException(errorResponse);

            return await GetUserThatContainsAsync(request.Email);
        }
        public async Task<AdminUserResponse> GetUserThatContainsAsync(string str)
        {
            var user = await authService.GetUserByLoginAsync(str);
            if (user == null) throw new KeyNotFoundException("User not found");

            var response = mapper.Map<AdminUserResponse>(user);
            response.Roles = await authService.GetUserRolesAsync(user);
            return response;
        }
        public async Task AdminUpdateUserAsync(AdminUserUpdateDataRequest request, CancellationToken cancellationToken)
        {
            var updateData = mapper.Map<UserUpdateData>(request);
            var user = await authService.GetUserByLoginAsync(request.CurrentLogin);

            var identityErrors = await authService.UpdateUserAsync(user, updateData, true);
            if (Utilities.HasErrors(identityErrors, out var errorResponse)) throw new AuthorizationException(errorResponse);

            identityErrors = await authService.SetUserRolesAsync(user, request.Roles);
            if (Utilities.HasErrors(identityErrors, out errorResponse)) throw new AuthorizationException(errorResponse);
        }
        public async Task AdminDeleteUserAsync(string login)
        {
            var user = await authService.GetUserByLoginAsync(login);
            var result = await authService.DeleteUserAsync(user);
            if (Utilities.HasErrors(result.Errors.ToList(), out var errorResponse)) throw new AuthorizationException(errorResponse);
        }

        #endregion
    }
}