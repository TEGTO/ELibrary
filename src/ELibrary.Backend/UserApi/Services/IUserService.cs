using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UserApi.Domain.Dtos;
using UserApi.Domain.Models;
using UserEntities.Domain.Entities;

namespace UserApi.Services
{
    public interface IUserService
    {
        public Task<IdentityResult> DeleteUserAsync(User user);
        public Task<IEnumerable<User>> GetPaginatedUsersAsync(AdminGetUserFilter filter, CancellationToken cancellationToken);
        public Task<User?> GetUserAsync(ClaimsPrincipal principal);
        /// <summary>
        /// Get a user by any information about him. 
        /// </summary>
        /// <param name="info">Login, Name, Email, Id, etc...</param>
        /// <returns></returns>
        public Task<User?> GetUserByUserInfoAsync(string info);
        public Task<List<string>> GetUserRolesAsync(User user);
        public Task<int> GetUserTotalAmountAsync(AdminGetUserFilter filter, CancellationToken cancellationToken);
        public Task<List<IdentityError>> SetUserRolesAsync(User user, List<string> roles);
        public Task<List<IdentityError>> UpdateUserAsync(User user, UserUpdateData updateData, bool resetPassword);
    }
}