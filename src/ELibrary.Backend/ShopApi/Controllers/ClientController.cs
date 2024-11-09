using Authentication.Identity;
using Caching.Services;
using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Features.ClientFeature.Command.CreateClient;
using ShopApi.Features.ClientFeature.Command.GetClient;
using ShopApi.Features.ClientFeature.Command.UpdateClient;
using ShopApi.Features.ClientFeature.Dtos;
using System.Security.Claims;

namespace ShopApi.Controllers
{
    [Authorize(Policy = Policy.REQUIRE_CLIENT_ROLE)]
    [Route("client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICacheService cacheService;

        public ClientController(IMediator mediator, ICacheService cacheService)
        {
            this.mediator = mediator;
            this.cacheService = cacheService;
        }

        #region Endpoints

        [HttpGet]
        public async Task<ActionResult<GetClientResponse>> GetClient(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cacheKey = $"GetClient_{userId}";
            var cachedResponse = cacheService.Get<GetClientResponse>(cacheKey);

            if (cachedResponse == null)
            {
                var response = await mediator.Send(new GetClientForUserQuery(userId), cancellationToken);
                cachedResponse = response;

                cacheService.Set(cacheKey, cachedResponse, TimeSpan.FromSeconds(10));
            }

            return Ok(cachedResponse);
        }
        [HttpPost]
        public async Task<ActionResult<ClientResponse>> CreateClient([FromBody] CreateClientRequest request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await mediator.Send(new CreateClientForUserCommand(userId, request), cancellationToken);

            return Created("", response);
        }
        [HttpPut]
        public async Task<ActionResult<ClientResponse>> UpdateClient([FromBody] UpdateClientRequest request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await mediator.Send(new UpdateClientForUserCommand(userId, request), cancellationToken);

            return Ok(response);
        }
        #endregion

        #region Admin Endpoints

        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpGet("admin/{userId}")]
        public async Task<ActionResult<GetClientResponse>> AdminGetClient(string userId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetClientForUserQuery(userId), cancellationToken);
            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPost("admin/{userId}")]
        public async Task<ActionResult<ClientResponse>> AdminCreateClient(string userId, [FromBody] CreateClientRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new CreateClientForUserCommand(userId, request), cancellationToken);
            return Created("", response);
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPut("admin/{userId}")]
        public async Task<ActionResult<ClientResponse>> AdminUpdateClient(string userId, [FromBody] UpdateClientRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new UpdateClientForUserCommand(userId, request), cancellationToken);
            return Ok(response);
        }

        #endregion
    }
}
