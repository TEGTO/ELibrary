using Authentication.Identity;
using AutoMapper;
using LibraryShopEntities.Domain.Dtos.Shop;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Domain.Dtos.Client;
using ShopApi.Services;
using System.Security.Claims;

namespace ShopApi.Controllers
{
    [Authorize]
    [Route("client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IClientService clientService;

        public ClientController(IMapper mapper, IClientService clientService)
        {
            this.mapper = mapper;
            this.clientService = clientService;
        }

        #region Endpoints

        [ResponseCache(Duration = 60)]
        [HttpGet]
        public async Task<ActionResult<ClientResponse>> GetClient(CancellationToken cancellationToken)
        {
            var id = GetUserId();
            var client = await clientService.GetClientByUserIdAsync(id, cancellationToken);

            if (client == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ClientResponse>(client));
        }
        [HttpPost]
        public async Task<ActionResult<ClientResponse>> CreateClient([FromBody] CreateClientRequest request, CancellationToken cancellationToken)
        {
            var id = GetUserId();
            var client = mapper.Map<Client>(request);
            client.UserId = id;
            var response = await clientService.CreateClientAsync(client, cancellationToken);
            return Created($"", mapper.Map<ClientResponse>(response));
        }
        [HttpPut]
        public async Task<ActionResult<ClientResponse>> UpdateClient([FromBody] UpdateClientRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var client = await clientService.GetClientByUserIdAsync(userId, cancellationToken);
            client.Copy(mapper.Map<Client>(request));
            var response = await clientService.UpdateClientAsync(client, cancellationToken);
            return Ok(mapper.Map<ClientResponse>(response));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteClient(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var client = await clientService.GetClientByUserIdAsync(userId, cancellationToken);
            await clientService.DeleteClientAsync(client.Id, cancellationToken);
            return Ok();
        }

        #endregion

        #region Admin Endpoints

        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpGet("admin/{id}")]
        public async Task<ActionResult<ClientResponse>> AdminGetClient(string id, CancellationToken cancellationToken)
        {
            var response = await clientService.GetClientByUserIdAsync(id, cancellationToken);
            return Ok(mapper.Map<ClientResponse>(response));
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPost("admin/{id}")]
        public async Task<ActionResult<ClientResponse>> AdminCreateClient(string id, [FromBody] CreateClientRequest request, CancellationToken cancellationToken)
        {
            var client = mapper.Map<Client>(request);
            client.UserId = id;
            var response = await clientService.CreateClientAsync(client, cancellationToken);
            return Created($"", mapper.Map<ClientResponse>(response));
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPut("admin/{id}")]
        public async Task<ActionResult<ClientResponse>> AdminUpdateClient(string id, [FromBody] UpdateClientRequest request, CancellationToken cancellationToken)
        {
            var client = await clientService.GetClientByUserIdAsync(id, cancellationToken);
            client.Copy(mapper.Map<Client>(request));
            var response = await clientService.UpdateClientAsync(client, cancellationToken);
            return Ok(mapper.Map<ClientResponse>(response));
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpDelete("admin/{id}")]
        public async Task<IActionResult> AdminDeleteClient(string id, CancellationToken cancellationToken)
        {
            var client = await clientService.GetClientByUserIdAsync(id, cancellationToken);
            await clientService.DeleteClientAsync(client.Id, cancellationToken);
            return Ok();
        }

        #endregion

        #region Private Helpers

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        #endregion
    }
}
