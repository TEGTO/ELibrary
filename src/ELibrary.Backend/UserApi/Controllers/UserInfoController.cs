using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;

namespace UserApi.Controllers
{
    [Route("userinfo")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IUserInfoService userInfoService;
        private readonly IMapper mapper;

        public UserInfoController(IUserInfoService userInfoService, IAuthService authService, IMapper mapper)
        {
            this.userInfoService = userInfoService;
            this.mapper = mapper;
            this.authService = authService;
        }
        [ResponseCache(Duration = 300)]
        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<GetCurrentUserResponse>> GetCurrentUser(CancellationToken cancellationToken)
        {
            var user = await authService.GetUserAsync(User);

            if (user == null)
            {
                return NotFound(new ResponseError
                {
                    StatusCode = "404",
                    Messages = new[] { "User not found." }
                });
            }

            var userInfo = await userInfoService.GetUserInfoAsync(user.Id, cancellationToken);

            var response = new GetCurrentUserResponse()
            {
                UserName = user.UserName,
                UserInfo = mapper.Map<UserInfoDto>(userInfo)
            };

            return Ok(response);
        }
    }
}