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
            ValidateQuery(request);

            var oathProvider = request.QueryParams.OAuthLoginProvider;

            string url = oAuthServices[oathProvider].GenerateOAuthRequestUrl(
                new GenerateOAuthRequestUrlParams(request.QueryParams.RedirectUrl!, request.QueryParams.CodeVerifier!));

            return Task.FromResult(new GetOAuthUrlResponse { Url = url });
        }

        private void ValidateQuery(GetOAuthUrlQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            var request = query.QueryParams;

            if (query.QueryParams == null) throw new ArgumentNullException(nameof(query.QueryParams));

            if (string.IsNullOrEmpty(request.RedirectUrl))
                throw new InvalidDataException("Redirect url can't be null or empty!");

            if (string.IsNullOrEmpty(request.CodeVerifier))
                throw new InvalidDataException("Code verifier can't be null or empty!");
        }
    }
}
