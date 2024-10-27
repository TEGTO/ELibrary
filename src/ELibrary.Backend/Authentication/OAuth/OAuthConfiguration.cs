using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AuthenticationTests")]
namespace Authentication.OAuth
{
    internal static class OAuthConfiguration
    {
        public const string GOOGLE_OAUTH_CLIENT_ID = "Auth:GoogleOAuth:ClientId";
        public const string GOOGLE_OAUTH_CLIENT_SECRET = "Auth:GoogleOAuth:ClientSecret";
        public const string GOOGLE_OAUTH_SCOPE = "Auth:GoogleOAuth:Scope";
    }
}
