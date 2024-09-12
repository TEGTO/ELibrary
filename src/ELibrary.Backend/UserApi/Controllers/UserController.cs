using Authentication.Identity;
using Authentication.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;
using UserApi.Domain.Models;
using UserApi.Services;
using UserEntities.Domain.Entities;

namespace UserApi.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAuthService authService;
        private readonly IConfiguration configuration;
        private readonly double expiryInDays;

        public UserController(IMapper mapper, IAuthService authService, IConfiguration configuration)
        {
            this.mapper = mapper;
            this.authService = authService;
            this.configuration = configuration;
            expiryInDays = double.Parse(configuration[Configuration.AUTH_REFRESH_TOKEN_EXPIRY_IN_DAYS]!);
        }

        #region Endpoints

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationRequest request)
        {
            var user = mapper.Map<User>(request);

            var errors = new List<IdentityError>();

            var registerParams = new RegisterUserParams(user, request.Password);
            errors.AddRange((await authService.RegisterUserAsync(registerParams)).Errors);
            if (Utilities.HasErrors(errors, out var errorResponse)) return errorResponse;

            errors.AddRange(await authService.SetUserRolesAsync(user, new() { Roles.CLIENT }));
            if (Utilities.HasErrors(errors, out errorResponse)) return errorResponse;

            return Created($"", null);
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserAuthenticationResponse>> Login([FromBody] UserAuthenticationRequest request)
        {
            var loginParams = new LoginUserParams(request.Login, request.Password, expiryInDays);
            var token = await authService.LoginUserAsync(loginParams);

            var tokenDto = mapper.Map<AuthToken>(token);
            var user = await authService.GetUserByLoginAsync(request.Login);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await authService.GetUserRolesAsync(user);

            var response = new UserAuthenticationResponse()
            {
                AuthToken = tokenDto,
                Email = user.Email,
                Roles = roles
            };

            return Ok(response);
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update(UserUpdateDataRequest request, CancellationToken cancellationToken)
        {
            UserUpdateData updateData = mapper.Map<UserUpdateData>(request);

            var user = await authService.GetUserAsync(User);
            var identityErrors = await authService.UpdateUserAsync(user, updateData, false);
            if (Utilities.HasErrors(identityErrors, out var errorResponse)) return errorResponse;

            return Ok();
        }
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete()
        {
            var user = await authService.GetUserAsync(User);
            var result = await authService.DeleteUserAsync(user);
            if (Utilities.HasErrors(result.Errors.ToList(), out var errorResponse)) return errorResponse;

            return Ok();
        }
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthToken>> Refresh(AuthToken request)
        {
            var tokenData = mapper.Map<AccessTokenData>(request);
            var newToken = await authService.RefreshTokenAsync(tokenData, expiryInDays);
            var tokenDto = mapper.Map<AuthToken>(newToken);
            return Ok(tokenDto);
        }

        #endregion

        #region Admin Endpoints

        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPost("admin/register")]
        public async Task<ActionResult<AdminUserResponse>> AdminRegister(AdminUserRegistrationRequest request)
        {
            var user = mapper.Map<User>(request);

            var errors = new List<IdentityError>();

            var registerParams = new RegisterUserParams(user, request.Password);
            errors.AddRange((await authService.RegisterUserAsync(registerParams)).Errors);
            if (Utilities.HasErrors(errors, out var errorResponse)) return errorResponse;

            errors.AddRange(await authService.SetUserRolesAsync(user, request.Roles));
            if (Utilities.HasErrors(errors, out errorResponse)) return errorResponse;

            return await GetUserByLogin(request.Email);
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpGet("admin/{login}")]
        public async Task<ActionResult<AdminUserResponse>> AdminGetUserByLogin(string login)
        {
            return await GetUserByLogin(login);
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPut("admin/update")]
        public async Task<IActionResult> AdminUpdate(AdminUserUpdateDataRequest request, CancellationToken cancellationToken)
        {
            UserUpdateData updateData = mapper.Map<UserUpdateData>(request);

            var user = await authService.GetUserByLoginAsync(request.CurrentLogin);
            var identityErrors = await authService.UpdateUserAsync(user, updateData, true);
            if (Utilities.HasErrors(identityErrors, out var errorResponse)) return errorResponse;

            identityErrors = await authService.SetUserRolesAsync(user, request.Roles);
            if (Utilities.HasErrors(identityErrors, out errorResponse)) return errorResponse;

            return Ok();
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpDelete("admin/delete/{login}")]
        public async Task<IActionResult> AdminDelete(string login)
        {
            var user = await authService.GetUserByLoginAsync(login);
            var result = await authService.DeleteUserAsync(user);
            if (Utilities.HasErrors(result.Errors.ToList(), out var errorResponse)) return errorResponse;

            return Ok();
        }

        #endregion

        #region Private Helpers

        private async Task<ActionResult<AdminUserResponse>> GetUserByLogin(string login)
        {
            var user = await authService.GetUserByLoginAsync(login);

            if (user == null)
            {
                return NotFound();
            }

            var response = mapper.Map<AdminUserResponse>(user);
            response.Roles = await authService.GetUserRolesAsync(user);

            return Ok(response);
        }

        #endregion
    }
}