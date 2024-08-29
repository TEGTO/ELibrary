using UserApi.Domain.Dtos;

namespace AuthenticationApi.Domain.Dtos
{
    public class UserRegistrationRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public UserInfoDto UserInfo { get; set; }
    }
}