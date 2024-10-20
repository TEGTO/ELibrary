﻿using Authentication.Identity;
using LibraryShopEntities.Domain.Dtos.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Features.ClientFeature.Dtos;
using ShopApi.Features.ClientFeature.Services;
using System.Security.Claims;

namespace ShopApi.Controllers
{
    [Authorize(Policy = Policy.REQUIRE_CLIENT_ROLE)]
    [Route("client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientManager clientManager;

        public ClientController(
            IClientManager clientManager)
        {
            this.clientManager = clientManager;
        }

        #region Endpoints

        [ResponseCache(Duration = 10)]
        [HttpGet]
        public async Task<ActionResult<GetClientResponse>> GetClient(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var clientResponse = await clientManager.GetClientForUserAsync(userId, cancellationToken);

            return Ok(clientResponse);
        }
        [HttpPost]
        public async Task<ActionResult<ClientResponse>> CreateClient([FromBody] CreateClientRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var response = await clientManager.CreateClientForUserAsync(userId, request, cancellationToken);
            return Created("", response);
        }
        [HttpPut]
        public async Task<ActionResult<ClientResponse>> UpdateClient([FromBody] UpdateClientRequest request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var response = await clientManager.UpdateClientForUserAsync(userId, request, cancellationToken);
            return Ok(response);
        }
        #endregion

        #region Admin Endpoints

        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpGet("admin/{id}")]
        public async Task<ActionResult<GetClientResponse>> AdminGetClient(string id, CancellationToken cancellationToken)
        {
            var response = await clientManager.GetClientForUserAsync(id, cancellationToken);
            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPost("admin/{id}")]
        public async Task<ActionResult<ClientResponse>> AdminCreateClient(string id, [FromBody] CreateClientRequest request, CancellationToken cancellationToken)
        {
            var response = await clientManager.CreateClientForUserAsync(id, request, cancellationToken);
            return Created("", response);
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPut("admin/{id}")]
        public async Task<ActionResult<ClientResponse>> AdminUpdateClient(string id, [FromBody] UpdateClientRequest request, CancellationToken cancellationToken)
        {
            var response = await clientManager.UpdateClientForUserAsync(id, request, cancellationToken);
            return Ok(response);
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
