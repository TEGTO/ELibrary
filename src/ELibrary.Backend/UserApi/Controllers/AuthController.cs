using Authentication.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.Command.Admin.AdminRegisterUser;
using UserApi.Command.Client.GetOAuthUrl;
using UserApi.Command.Client.LoginOAuth;
using UserApi.Command.Client.LoginUser;
using UserApi.Command.Client.RefreshToken;
using UserApi.Command.Client.RegisterUser;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.Controllers
{
    [ResponseCache(Duration = 3)]
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        #region Endpoints

        [HttpPost("register")]
        public async Task<ActionResult<UserAuthenticationResponse>> Register(UserRegistrationRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new RegisterUserCommand(request), cancellationToken);
            return CreatedAtAction(nameof(Register), new { id = response.Email }, response);
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserAuthenticationResponse>> Login(UserAuthenticationRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new LoginUserCommand(request), cancellationToken);
            return Ok(response);
        }
        [HttpGet("oauth")]
        public async Task<ActionResult<GetOAuthUrlResponse>> GetOAuthUrl([FromQuery] GetOAuthUrlQueryParams queryParams, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetOAuthUrlQuery(queryParams), cancellationToken);
            return Ok(response);
        }
        [HttpPost("oauth")]
        public async Task<ActionResult<UserAuthenticationResponse>> LoginOAuth(LoginOAuthRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new LoginOAuthCommand(request), cancellationToken);
            return Ok(response);
        }
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthToken>> Refresh(AuthToken request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new RefreshTokenCommand(request), cancellationToken);
            return Ok(response);
        }

        #endregion

        #region Admin Endpoints

        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPost("admin/register")]
        public async Task<ActionResult<AdminUserResponse>> AdminRegister(AdminUserRegistrationRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new AdminRegisterUserCommand(request), cancellationToken);
            return Ok(response);
        }

        #endregion
    }
}
