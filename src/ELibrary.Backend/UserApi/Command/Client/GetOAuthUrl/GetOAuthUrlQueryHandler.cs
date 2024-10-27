using Authentication.OAuth;
using MediatR;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services.OAuth;

namespace UserApi.Command.Client.GetOAuthUrl
{
    public class GetOAuthUrlQueryHandler : IRequestHandler<GetOAuthUrlQuery, GetOAuthUrlResponse>
    {
        private readonly Dictionary<OAuthLoginProvider, IOAuthService> oAuthServices;

        public GetOAuthUrlQueryHandler(Dictionary<OAuthLoginProvider, IOAuthService> oAuthServices)
        {
            this.oAuthServices = oAuthServices;
        }

        public Task<GetOAuthUrlResponse> Handle(GetOAuthUrlQuery request, CancellationToken cancellationToken)
        {
            var oathProvider = request.QueryParams.OAuthLoginProvider;

            string url = oAuthServices[oathProvider].GenerateOAuthRequestUrl(
                new GenerateOAuthRequestUrlParams(request.QueryParams.RedirectUrl, request.QueryParams.CodeVerifier));

            return Task.FromResult(new GetOAuthUrlResponse { Url = url });
        }
    }
}
