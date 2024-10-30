using Authentication.Identity;
using Authentication.Models;
using Authentication.Token;
using Microsoft.AspNetCore.Identity;

namespace LibraryApi.IntegrationTests.Controllers
{
    internal abstract class BaseControllerTest : BaseIntegrationTest
    {
        private string managerAccessToken = string.Empty;
        private string accessToken = string.Empty;

        protected string ManagerAccessToken
        {
            get
            {
                if (string.IsNullOrEmpty(managerAccessToken))
                {
                    managerAccessToken = GetManagerAccessTokenData().AccessToken;
                }
                return managerAccessToken;
            }
        }
        protected string AccessToken
        {
            get
            {
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = GetAccessTokenData().AccessToken;
                }
                return accessToken;
            }
        }

        protected AccessTokenData GetManagerAccessTokenData()
        {
            var jwtHandler = new JwtHandler(settings);
            IdentityUser identity = new IdentityUser()
            {
                Id = "1",
                UserName = "testuser",
                Email = "test@example.com"
            };
            return jwtHandler.CreateToken(identity, [Roles.MANAGER]);
        }
        protected AccessTokenData GetAccessTokenData()
        {
            var jwtHandler = new JwtHandler(settings);
            IdentityUser identity = new IdentityUser()
            {
                Id = "1",
                UserName = "testuser",
                Email = "test@example.com"
            };
            return jwtHandler.CreateToken(identity, [Roles.CLIENT]);
        }
    }
}
