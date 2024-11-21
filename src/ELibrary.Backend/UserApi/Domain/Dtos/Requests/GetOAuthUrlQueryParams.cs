using Authentication.OAuth;

namespace UserApi.Domain.Dtos.Requests
{
    public class GetOAuthUrlQueryParams
    {
        public OAuthLoginProvider OAuthLoginProvider { get; set; }
        public string? RedirectUrl { get; set; }
        public string? CodeVerifier { get; set; }
    }
}
