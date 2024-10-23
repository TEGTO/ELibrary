using MediatR;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.Command.Client.GetOAuthUrl
{
    public record GetOAuthUrlQuery(GetOAuthUrlQueryParams QueryParams) : IRequest<GetOAuthUrlResponse>;
}
