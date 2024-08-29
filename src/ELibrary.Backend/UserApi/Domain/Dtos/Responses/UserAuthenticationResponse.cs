using AuthenticationApi.Domain.Dtos;
using UserApi.Domain.Dtos;

namespace UserApi.Domain.Dtos.Responses
{
    public class UserAuthenticationResponse
    {
        public AuthToken AuthToken { get; set; }
        public string UserName { get; set; }
        public UserInfoDto UserInfo { get; set; }
    }
}
