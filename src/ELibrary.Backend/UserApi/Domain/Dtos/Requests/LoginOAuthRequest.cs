using Authentication.OAuth;

namespace UserApi.Domain.Dtos.Requests
{
    public class LoginOAuthRequest
    {
        public string Code { get; set; } = string.Empty;
        public string CodeVerifier { get; set; } = string.Empty;
        public string RedirectUrl { get; set; } = string.Empty;
        public OAuthLoginProvider OAuthLoginProvider { get; set; }
    }
}
