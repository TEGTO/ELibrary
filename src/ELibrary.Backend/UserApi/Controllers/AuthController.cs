using Authentication.Models;
using AuthenticationApi.Domain.Dtos;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using System.Net;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Responses;

namespace AuthenticationApi.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAuthService authService;
        private readonly IConfiguration configuration;
        private readonly double expiryInDays;

        public AuthController(IMapper mapper, IAuthService authService, IConfiguration configuration)
        {
            this.mapper = mapper;
            this.authService = authService;
            this.configuration = configuration;
            expiryInDays = double.Parse(configuration[Configuration.AUTH_REFRESH_TOKEN_EXPIRY_IN_DAYS]!);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request!");
            }

            var user = mapper.Map<User>(request);

            var result = await authService.RegisterUserAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return BadRequest(new ResponseError
                {
                    StatusCode = ((int)HttpStatusCode.BadRequest).ToString(),
                    Messages = errors
                });
            }

            return Created($"/users/{user.Id}", null);
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserAuthenticationResponse>> Login([FromBody] UserAuthenticationRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request!");
            }

            var token = await authService.LoginUserAsync(request.Login, request.Password, expiryInDays);

            var tokenDto = mapper.Map<AuthToken>(token);
            var user = await authService.GetUserByLoginAsync(request.Login);

            var response = new UserAuthenticationResponse()
            {
                AuthToken = tokenDto,
                UserName = user.UserName,
                UserInfo = mapper.Map<UserInfoDto>(user.UserInfo)
            };

            return Ok(response);
        }
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthToken>> Refresh([FromBody] AuthToken request)
        {
            var tokenData = mapper.Map<AccessTokenData>(request);
            var newToken = await authService.RefreshTokenAsync(tokenData, expiryInDays);
            var tokenDto = mapper.Map<AuthToken>(newToken);
            return Ok(tokenDto);
        }
    }
}