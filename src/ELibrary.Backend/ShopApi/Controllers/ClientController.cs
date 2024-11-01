using Authentication.Identity;
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

        public ClientController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        #region Endpoints

        [ResponseCache(Duration = 10)]
        [HttpGet]
        public async Task<ActionResult<GetClientResponse>> GetClient(CancellationToken cancellationToken)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await mediator.Send(new GetClientQuery(userId), cancellationToken);

            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<ClientResponse>> CreateClient([FromBody] CreateClientRequest request, CancellationToken cancellationToken)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await mediator.Send(new CreateClientCommand(userId, request), cancellationToken);

            return Created("", response);
        }
        [HttpPut]
        public async Task<ActionResult<ClientResponse>> UpdateClient([FromBody] UpdateClientRequest request, CancellationToken cancellationToken)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await mediator.Send(new UpdateClientCommand(userId, request), cancellationToken);

            return Created("", response);
        }
        #endregion

        #region Admin Endpoints

        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpGet("admin/{id}")]
        public async Task<ActionResult<GetClientResponse>> AdminGetClient(string id, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetClientQuery(id), cancellationToken);
            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPost("admin/{id}")]
        public async Task<ActionResult<ClientResponse>> AdminCreateClient(string id, [FromBody] CreateClientRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new CreateClientCommand(id, request), cancellationToken);
            return Created("", response);
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPut("admin/{id}")]
        public async Task<ActionResult<ClientResponse>> AdminUpdateClient(string id, [FromBody] UpdateClientRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new UpdateClientCommand(id, request), cancellationToken);
            return Ok(response);
        }

        #endregion
    }
}
