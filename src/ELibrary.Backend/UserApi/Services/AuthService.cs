using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Models;
using UserEntities.Domain.Entities;

namespace UserApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ITokenHandler tokenHandler;

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ITokenHandler tokenHandler)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.tokenHandler = tokenHandler;
        }

        #region IAuthService Members

        public async Task<IdentityResult> RegisterUserAsync(RegisterUserParams registerParams)
        {
            return await userManager.CreateAsync(registerParams.User, registerParams.Password);
        }
        public async Task<AccessTokenData> LoginUserAsync(LoginUserParams loginParams)
        {
            var user = await GetUserByUserInfoAsync(loginParams.Login);

            if (user == null || !await userManager.CheckPasswordAsync(user, loginParams.Password))
            {
                throw new UnauthorizedAccessException("Invalid authentication. Login is not correct.");
            }

            var tokenData = await CreateNewTokenDataAsync(user, loginParams.RefreshTokenExpiryInDays);
            await SetRefreshToken(user, tokenData);

            return tokenData;
        }
        public async Task<User?> GetUserAsync(ClaimsPrincipal principal)
        {
            var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            return id.IsNullOrEmpty() ? null : await userManager.FindByIdAsync(id!);
        }
        public async Task<User?> GetUserByUserInfoAsync(string info)
        {
            var user = await userManager.FindByEmailAsync(info);
            user = user == null ? await userManager.FindByNameAsync(info) : user;
            user = user == null ? await userManager.FindByIdAsync(info) : user;
            return user;
        }
        public async Task<IEnumerable<User>> GetPaginatedUsersAsync(GetUserFilterRequest filter, CancellationToken cancellationToken)
        {
            var queryable = userManager.Users.AsNoTracking();
            List<User> paginatedUsers = new List<User>();

            queryable = ApplyFilter(queryable, filter);

            paginatedUsers.AddRange(await queryable
                  .Skip((filter.PageNumber - 1) * filter.PageSize)
                  .Take(filter.PageSize)
                  .ToListAsync(cancellationToken));

            return paginatedUsers;
        }
        public async Task<int> GetUserTotalAmountAsync(GetUserFilterRequest filter, CancellationToken cancellationToken)
        {
            var queryable = userManager.Users.AsNoTracking();

            queryable = ApplyFilter(queryable, filter);

            return await queryable.CountAsync(cancellationToken);
        }
        public async Task<List<IdentityError>> SetUserRolesAsync(User user, List<string> roles)
        {
            List<IdentityError> identityErrors = new List<IdentityError>();

            var currentRoles = await userManager.GetRolesAsync(user);

            var removeResult = await userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                identityErrors.AddRange(removeResult.Errors);
                return identityErrors;
            }

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var createRoleResult = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!createRoleResult.Succeeded)
                    {
                        identityErrors.AddRange(createRoleResult.Errors);
                        continue;
                    }
                }

                var addResult = await userManager.AddToRoleAsync(user, role);
                if (!addResult.Succeeded)
                {
                    identityErrors.AddRange(addResult.Errors);
                }
            }

            return identityErrors;
        }
        public async Task<List<string>> GetUserRolesAsync(User user)
        {
            return (await userManager.GetRolesAsync(user)).ToList();
        }
        public async Task<List<IdentityError>> UpdateUserAsync(User user, UserUpdateData updateData, bool resetPassword)
        {
            List<IdentityError> identityErrors = new List<IdentityError>();

            if (!user.UserName.Equals(updateData.UserName))
            {
                var result = await userManager.SetUserNameAsync(user, updateData.UserName);
                identityErrors.AddRange(result.Errors);
            }

            if (!user.Email.Equals(updateData.Email))
            {
                var token = await userManager.GenerateChangeEmailTokenAsync(user, updateData.Email);
                var result = await userManager.ChangeEmailAsync(user, updateData.Email, token);
                identityErrors.AddRange(result.Errors);
            }

            if (!string.IsNullOrEmpty(updateData.Password))
            {
                if (resetPassword)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await userManager.ResetPasswordAsync(user, token, updateData.Password);
                    identityErrors.AddRange(result.Errors);
                }
                else
                {
                    var result = await userManager.ChangePasswordAsync(user, updateData.OldPassword, updateData.Password);
                    identityErrors.AddRange(result.Errors);
                }
            }

            return RemoveDuplicates(identityErrors);
        }
        public async Task<AccessTokenData> RefreshTokenAsync(AccessTokenData accessTokenData, double refreshTokenExpiryInDays)
        {
            var principal = tokenHandler.GetPrincipalFromExpiredToken(accessTokenData.AccessToken);
            var user = await userManager.FindByNameAsync(principal.Identity.Name);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid authentication. AccessToken is not valid.");
            }

            if (user.RefreshToken != accessTokenData.RefreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new InvalidDataException("Refresh token is not valid!");
            }

            var tokenData = await CreateNewTokenDataAsync(user, refreshTokenExpiryInDays);
            await SetRefreshToken(user, tokenData);

            return tokenData;
        }
        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            var result = await userManager.DeleteAsync(user);
            return result;
        }

        #endregion

        #region Private Helpers

        private async Task<AccessTokenData> CreateNewTokenDataAsync(User user, double refreshTokenExpiryInDays)
        {
            var roles = await userManager.GetRolesAsync(user);
            var tokenData = tokenHandler.CreateToken(user, roles);
            tokenData.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(refreshTokenExpiryInDays);
            return tokenData;
        }
        private async Task SetRefreshToken(User user, AccessTokenData accessTokenData)
        {
            user.RefreshToken = accessTokenData.RefreshToken;
            user.RefreshTokenExpiryTime = accessTokenData.RefreshTokenExpiryDate;
            await userManager.UpdateAsync(user);
        }
        private List<IdentityError> RemoveDuplicates(List<IdentityError> identityErrors)
        {
            identityErrors = identityErrors
            .GroupBy(e => e.Description)
            .Select(g => g.First())
            .ToList();
            return identityErrors;
        }
        private IQueryable<User> ApplyFilter(IQueryable<User> query, GetUserFilterRequest userFilter)
        {
            if (!string.IsNullOrEmpty(userFilter.ContainsString))
            {
                query = query.Where(b =>
                   b.Email.Contains(userFilter.ContainsString)
                || b.UserName.Contains(userFilter.ContainsString)
                || b.Id.Contains(userFilter.ContainsString)
                );
            }
            return query
                 .OrderByDescending(b => b.RegistredAtUtc);
        }

        #endregion
    }
}