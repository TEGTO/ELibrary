using Authentication.OAuth;

namespace UserApi.Domain.Dtos.Requests
{
    public class LoginOAuthRequest
    {
        public string? Code { get; set; }
        public string? CodeVerifier { get; set; }
        public string? RedirectUrl { get; set; }
        public OAuthLoginProvider OAuthLoginProvider { get; set; }
    }
}
