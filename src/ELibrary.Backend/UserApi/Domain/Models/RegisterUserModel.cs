using UserEntities.Domain.Entities;

namespace UserApi.Domain.Models
{
    public record class RegisterUserModel(User User, string Password);
}