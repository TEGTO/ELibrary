using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserApi.Domain.Dtos;
using UserApi.Domain.Models;
using UserEntities.Domain.Entities;

namespace UserApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        #region IUserService Members

        public async Task<User?> GetUserAsync(ClaimsPrincipal principal)
        {
            var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(id) ? null : await userManager.FindByIdAsync(id!);
        }
        public async Task<User?> GetUserByUserInfoAsync(string info)
        {
            var user = await userManager.FindByEmailAsync(info);
            user = user == null ? await userManager.FindByNameAsync(info) : user;
            user = user == null ? await userManager.FindByIdAsync(info) : user;
            return user;
        }
        public async Task<IEnumerable<User>> GetPaginatedUsersAsync(AdminGetUserFilter filter, CancellationToken cancellationToken)
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
        public async Task<int> GetUserTotalAmountAsync(AdminGetUserFilter filter, CancellationToken cancellationToken)
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

            if (user.UserName != null && !user.UserName.Equals(updateData.UserName))
            {
                var result = await userManager.SetUserNameAsync(user, updateData.UserName);
                identityErrors.AddRange(result.Errors);
            }

            if (user.Email != null && !user.Email.Equals(updateData.Email))
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
        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            var result = await userManager.DeleteAsync(user);
            return result;
        }

        #endregion

        #region Private Helpers

        private IQueryable<User> ApplyFilter(IQueryable<User> query, AdminGetUserFilter userFilter)
        {
            if (!string.IsNullOrEmpty(userFilter.ContainsInfo))
            {
                query = query.Where(b =>
                    b.Email != null && b.Email.Contains(userFilter.ContainsInfo)
                    || b.UserName != null && b.UserName.Contains(userFilter.ContainsInfo)
                    || b.Id.Contains(userFilter.ContainsInfo)
                );
            }
            return query
                 .OrderByDescending(b => b.RegistredAtUtc);
        }
        private List<IdentityError> RemoveDuplicates(List<IdentityError> identityErrors)
        {
            identityErrors = identityErrors
            .GroupBy(e => e.Description)
            .Select(g => g.First())
            .ToList();
            return identityErrors;
        }

        #endregion

    }
}
