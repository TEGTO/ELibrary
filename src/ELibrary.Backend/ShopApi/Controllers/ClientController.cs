using Authentication.Identity;
using LibraryShopEntities.Domain.Dtos.Shop;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
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

        public ClientController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        #region Endpoints

        [HttpGet]
        [OutputCache(PolicyName = "ClientPolicy")]
        public async Task<ActionResult<GetClientResponse>> GetClient(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await mediator.Send(new GetClientForUserQuery(userId), cancellationToken);

            return Ok(response);
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
