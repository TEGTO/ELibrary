using Authentication.OAuth;

namespace UserApi.Domain.Dtos.Requests
{
    public class GetOAuthUrlQueryParams
    {
        public OAuthLoginProvider OAuthLoginProvider { get; set; }
        public string RedirectUrl { get; set; } = string.Empty;
        public string CodeVerifier { get; set; } = string.Empty;
    }
}
