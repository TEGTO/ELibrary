using MediatR;
using ShopApi.Features.ClientFeature.Dtos;

namespace ShopApi.Features.ClientFeature.Command.GetClient
{
    public record GetClientForUserQuery(string UserId) : IRequest<GetClientResponse>;
}
