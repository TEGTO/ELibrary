using Authentication.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;
using UserApi.Services;

namespace UserApi.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;

        public UserController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        #region Endpoints

        [HttpPost("register")]
        public async Task<ActionResult<UserAuthenticationResponse>> Register(UserRegistrationRequest request)
        {
            var response = await userManager.RegisterUserAsync(request);
            return CreatedAtAction(nameof(Register), new { id = response.Email }, response);
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserAuthenticationResponse>> Login([FromBody] UserAuthenticationRequest request)
        {
            var response = await userManager.LoginUserAsync(request);
            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_CLIENT_ROLE)]
        [HttpPut("update")]
        public async Task<IActionResult> Update(UserUpdateDataRequest request, CancellationToken cancellationToken)
        {
            await userManager.UpdateUserAsync(request, User, cancellationToken);
            return Ok();
        }
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthToken>> Refresh(AuthToken request)
        {
            var token = await userManager.RefreshTokenAsync(request);
            return Ok(token);
        }

        #endregion

        #region Admin Endpoints

        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPost("admin/register")]
        public async Task<ActionResult<AdminUserResponse>> AdminRegister(AdminUserRegistrationRequest request)
        {
            var response = await userManager.AdminRegisterUserAsync(request);
            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpGet("admin/users/{info}")]
        public async Task<ActionResult<AdminUserResponse>> AdminGetUser(string info)
        {
            var response = await userManager.GetUserByInfoAsync(info);
            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPost("admin/users")]
        public async Task<ActionResult<IEnumerable<AdminUserResponse>>> AdminGetPaginatedUsers(GetUserFilterRequest request, CancellationToken cancellationToken)
        {
            var response = await userManager.GetPaginatedUsersAsync(request, cancellationToken);
            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPost("admin/users/amount")]
        public async Task<ActionResult<int>> AdminGetPaginatedUserAmount(GetUserFilterRequest request, CancellationToken cancellationToken)
        {
            var response = await userManager.GetPaginatedUserTotalAmountAsync(request, cancellationToken);
            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPut("admin/update")]
        public async Task<IActionResult> AdminUpdate(AdminUserUpdateDataRequest request, CancellationToken cancellationToken)
        {
            await userManager.AdminUpdateUserAsync(request, cancellationToken);
            return Ok();
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpDelete("admin/delete/{info}")]
        public async Task<IActionResult> AdminDelete(string info)
        {
            await userManager.AdminDeleteUserAsync(info);
            return Ok();
        }

        #endregion
    }
}