using Authentication.Identity;
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
            var user = await authService.GetUserByUserInfoAsync(request.Login);
            if (user == null) throw new UnauthorizedAccessException("Invalid authentication! Wrong password or login!");

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
        public async Task UpdateUserAsync(UserUpdateDataRequest request, ClaimsPrincipal userPricipal, CancellationToken cancellationToken)
        {
            var updateData = mapper.Map<UserUpdateData>(request);
            var user = await authService.GetUserAsync(userPricipal);

            var identityErrors = await authService.UpdateUserAsync(user, updateData, false);
            if (Utilities.HasErrors(identityErrors, out var errorResponse)) throw new AuthorizationException(errorResponse);
        }
        public async Task<AuthToken> RefreshTokenAsync(AuthToken request)
        {
            var tokenData = mapper.Map<AccessTokenData>(request);
            var newToken = await authService.RefreshTokenAsync(tokenData, expiryInDays);
            return mapper.Map<AuthToken>(newToken);
        }
        public async Task DeleteUserAsync(ClaimsPrincipal userPricipal, CancellationToken cancellationToken)
        {
            var user = await authService.GetUserAsync(userPricipal);
            await authService.DeleteUserAsync(user);
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

            return await GetUserByInfoAsync(request.Email);
        }
        public async Task<AdminUserResponse> GetUserByInfoAsync(string info)
        {
            var user = await authService.GetUserByUserInfoAsync(info);

            if (user == null)
            {
                throw new KeyNotFoundException("User is not found!");
            }

            var response = mapper.Map<AdminUserResponse>(user);
            response.Roles = await authService.GetUserRolesAsync(user);
            return response;
        }
        public async Task<IEnumerable<AdminUserResponse>> GetPaginatedUsersAsync(AdminGetUserFilter filter, CancellationToken cancellationToken)
        {
            var users = await authService.GetPaginatedUsersAsync(filter, cancellationToken);

            var responses = new List<AdminUserResponse>();

            foreach (var user in users)
            {
                var response = mapper.Map<AdminUserResponse>(user);
                response.Roles = await authService.GetUserRolesAsync(user);
                responses.Add(response);
            }

            return responses;
        }
        public async Task<int> GetPaginatedUserTotalAmountAsync(AdminGetUserFilter filter, CancellationToken cancellationToken)
        {
            return await authService.GetUserTotalAmountAsync(filter, cancellationToken);
        }
        public async Task<AdminUserResponse> AdminUpdateUserAsync(AdminUserUpdateDataRequest request, CancellationToken cancellationToken)
        {
            var updateData = mapper.Map<UserUpdateData>(request);
            var user = await authService.GetUserByUserInfoAsync(request.CurrentLogin);

            var identityErrors = await authService.UpdateUserAsync(user, updateData, true);
            if (Utilities.HasErrors(identityErrors, out var errorResponse)) throw new AuthorizationException(errorResponse);

            identityErrors = await authService.SetUserRolesAsync(user, request.Roles);
            if (Utilities.HasErrors(identityErrors, out errorResponse)) throw new AuthorizationException(errorResponse);

            var response = mapper.Map<AdminUserResponse>(user);
            response.Roles = await authService.GetUserRolesAsync(user);

            return response;
        }
        public async Task AdminDeleteUserAsync(string info)
        {
            var user = await authService.GetUserByUserInfoAsync(info);
            var result = await authService.DeleteUserAsync(user);
            if (Utilities.HasErrors(result.Errors.ToList(), out var errorResponse)) throw new AuthorizationException(errorResponse);
        }

        #endregion
    }
}
