using Microsoft.AspNetCore.Identity;
using UserApi.Domain.Models;
using UserEntities.Data;
using UserEntities.Domain.Entities;

namespace UserApi
{
    public static class CreateUserFromOAuthExtension
    {
        public static async Task<User> CreateUserFromOAuth(this UserManager<User> userManager, UserIdentityDbContext context, CreateUserFromOAuth model)
        {
            var user = await userManager.FindByLoginAsync(nameof(model.LoginProvider), model.LoginProviderSubject);

            if (user != null)
                return user;

            user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    LoginProvider = model.LoginProvider
                };

                await userManager.CreateAsync(user);

                user.EmailConfirmed = true;

                await userManager.UpdateAsync(user);
                await context.SaveChangesAsync();
            }

            UserLoginInfo userLoginInfo = null;
            switch (model.LoginProvider)
            {
                case LoginProvider.GoogleOAuth:
                    {
                        userLoginInfo = new UserLoginInfo(nameof(model.LoginProvider), model.LoginProviderSubject, nameof(model.LoginProvider).ToUpper());
                    }
                    break;
                default:
                    break;
            }

            var result = await userManager.AddLoginAsync(user, userLoginInfo);

            if (result.Succeeded)
                return user;

            else
                return null;
        }
    }
}
