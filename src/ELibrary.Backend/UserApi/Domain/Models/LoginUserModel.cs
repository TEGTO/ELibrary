using UserEntities.Domain.Entities;

namespace UserApi.Domain.Models
{
    public record class LoginUserModel(User User, string Password);
}