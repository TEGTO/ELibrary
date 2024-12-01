using Authentication.Models;
using UserEntities.Domain.Entities;

namespace UserApi.Domain.Models
{
    public record class RefreshTokenModel(User User, AccessTokenData AccessTokenData);
}