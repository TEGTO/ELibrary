using Microsoft.AspNetCore.Identity;
using UserApi.Domain.Models;
using UserEntities.Domain.Entities;

namespace UserApi.Services.OAuth
{
    public class UserOAuthCreationService : IUserOAuthCreationService
    {
        private readonly UserManager<User> userManager;
        private readonly IUserAuthenticationMethodService authMethodService;

        public UserOAuthCreationService(UserManager<User> userManager, IUserAuthenticationMethodService authMethodService)
        {
            this.userManager = userManager;
            this.authMethodService = authMethodService;
        }

        public async Task<User?> CreateUserFromOAuthAsync(CreateUserFromOAuth model, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByLoginAsync(nameof(model.AuthMethod), model.LoginProviderSubject);

            if (user != null)
                return user;

            user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user);
            }

            UserLoginInfo userLoginInfo = null;
            switch (model.AuthMethod)
            {
                case AuthenticationMethod.GoogleOAuth:
                    userLoginInfo = new UserLoginInfo(nameof(model.AuthMethod), model.LoginProviderSubject, nameof(model.AuthMethod).ToUpper());
                    break;
                default:
                    break;
            }

            await authMethodService.SetUserAuthenticationMethodAsync(user, model.AuthMethod, cancellationToken);

            var result = await userManager.AddLoginAsync(user, userLoginInfo);

            return result.Succeeded ? user : null;
        }
    }
}
