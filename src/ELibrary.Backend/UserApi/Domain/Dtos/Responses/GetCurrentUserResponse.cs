namespace UserApi.Domain.Dtos.Responses
{
    public class GetCurrentUserResponse
    {
        public string UserName { get; set; }
        public UserInfoDto UserInfo { get; set; }
    }
}
