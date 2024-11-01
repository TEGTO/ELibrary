using MediatR;
using ShopApi.Features.ClientFeature.Dtos;

namespace ShopApi.Features.ClientFeature.Command.GetClient
{
    public record GetClientQuery(string UserId) : IRequest<GetClientResponse>;
}
