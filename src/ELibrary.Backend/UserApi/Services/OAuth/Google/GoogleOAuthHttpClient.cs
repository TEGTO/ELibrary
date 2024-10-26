﻿using Authentication.OAuth.Google;
using Microsoft.AspNetCore.WebUtilities;
using Shared.Helpers;

namespace UserApi.Services.OAuth.Google
{
    public class GoogleOAuthHttpClient : IGoogleOAuthHttpClient
    {
        private const string TOKEN_SERVER_ENDPOINT = "https://oauth2.googleapis.com/token";
        private const string OAUTH_SERVER_ENDPOINT = "https://accounts.google.com/o/oauth2/v2/auth";
        private readonly IHttpHelper httpHelperService;
        private readonly GoogleOAuthSettings oAuthSettings;

        public GoogleOAuthHttpClient(GoogleOAuthSettings oAuthSettings, IHttpHelper httpHelperService)
        {
            this.oAuthSettings = oAuthSettings;
            this.httpHelperService = httpHelperService;
        }

        public async Task<GoogleOAuthTokenResult?> ExchangeAuthorizationCodeAsync(string code, string codeVerifier, string redirectUrl, CancellationToken cancellationToken)
        {
            var authParams = new Dictionary<string, string>
            {
                { "client_id", oAuthSettings.ClientId },
                { "client_secret", oAuthSettings.ClientSecret },
                { "code", code },
                { "code_verifier", codeVerifier },
                { "grant_type", "authorization_code" },
                { "redirect_uri", redirectUrl }
            };

            return await httpHelperService.SendPostRequestAsync<GoogleOAuthTokenResult>(TOKEN_SERVER_ENDPOINT, authParams, cancellationToken: cancellationToken);
        }
        public string GenerateOAuthRequestUrl(string scope, string redirectUrl, string codeVerifier)
        {
            var codeChallenge = HashHelper.ComputeHash(codeVerifier);

            var queryParams = new Dictionary<string, string?>
            {
                {"client_id", oAuthSettings.ClientId },
                {"redirect_uri", redirectUrl },
                {"response_type", "code" },
                {"scope", scope },
                {"code_challenge", codeChallenge },
                {"code_challenge_method", "S256" },
                {"access_type", "offline" }
            };

            var url = QueryHelpers.AddQueryString(OAUTH_SERVER_ENDPOINT, queryParams);
            return url;
        }

        public async Task<GoogleOAuthTokenResult?> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var refreshParams = new Dictionary<string, string>
            {
                { "client_id", oAuthSettings.ClientId },
                { "client_secret", oAuthSettings.ClientSecret },
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken }
            };

            return await httpHelperService.SendPostRequestAsync<GoogleOAuthTokenResult>(TOKEN_SERVER_ENDPOINT, refreshParams, cancellationToken: cancellationToken);
        }
    }
}