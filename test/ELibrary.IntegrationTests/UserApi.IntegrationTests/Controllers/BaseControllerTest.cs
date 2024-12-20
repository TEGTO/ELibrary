﻿using Authentication.Identity;
using Authentication.Models;
using Authentication.Token;
using Microsoft.AspNetCore.Identity;

namespace UserApi.IntegrationTests.Controllers
{
    internal abstract class BaseControllerTest : BaseIntegrationTest
    {
        private string adminAccessToken = string.Empty;
        private string accessToken = string.Empty;

        protected string AdminAccessToken
        {
            get
            {
                if (string.IsNullOrEmpty(adminAccessToken))
                {
                    adminAccessToken = GetAdminAccessTokenData().AccessToken ?? "";
                }
                return adminAccessToken;
            }
        }
        protected string AccessToken
        {
            get
            {
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = GetAccessTokenData().AccessToken ?? "";
                }
                return accessToken;
            }
        }

        protected AccessTokenData GetAdminAccessTokenData()
        {
            var jwtHandler = new JwtHandler(settings);

            IdentityUser identity = new IdentityUser()
            {
                Id = "1",
                UserName = "testuser",
                Email = "test@example.com"
            };

            return jwtHandler.CreateToken(identity, [Roles.ADMINISTRATOR]);
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
