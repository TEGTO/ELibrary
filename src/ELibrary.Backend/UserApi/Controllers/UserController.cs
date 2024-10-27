using Authentication.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.Command.Admin.AdminDeleteUser;
using UserApi.Command.Admin.AdminUpdateUser;
using UserApi.Command.Admin.GetPaginatedUsers;
using UserApi.Command.Admin.GetPaginatedUserTotalAmount;
using UserApi.Command.Admin.GetUserByInfo;
using UserApi.Command.Client.DeleteUser;
using UserApi.Command.Client.UpdateUser;
using UserApi.Domain.Dtos;
using UserApi.Domain.Dtos.Requests;
using UserApi.Domain.Dtos.Responses;

namespace UserApi.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        #region Endpoints

        [Authorize(Policy = Policy.REQUIRE_CLIENT_ROLE)]
        [HttpPut("update")]
        public async Task<IActionResult> Update(UserUpdateDataRequest request, CancellationToken cancellationToken)
        {
            await mediator.Send(new UpdateUserCommand(request, User), cancellationToken);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteUserCommand(User), cancellationToken);
            return Ok();
        }

        #endregion

        #region Admin Endpoints

        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpGet("admin/users/{info}")]
        public async Task<ActionResult<AdminUserResponse>> AdminGetUser(string info, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetUserByInfoQuery(info), cancellationToken);
            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPost("admin/users")]
        public async Task<ActionResult<IEnumerable<AdminUserResponse>>> AdminGetPaginatedUsers(AdminGetUserFilter request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetPaginatedUsersQuery(request), cancellationToken);
            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPost("admin/users/amount")]
        public async Task<ActionResult<int>> AdminGetPaginatedUserAmount(AdminGetUserFilter request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetPaginatedUserTotalAmountQuery(request), cancellationToken);
            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpPut("admin/update")]
        public async Task<ActionResult<AdminUserResponse>> AdminUpdate(AdminUserUpdateDataRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new AdminUpdateUserCommand(request), cancellationToken);
            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_ADMIN_ROLE)]
        [HttpDelete("admin/delete/{info}")]
        public async Task<IActionResult> AdminDeleteUser(string info, CancellationToken cancellationToken)
        {
            await mediator.Send(new AdminDeleteUserCommand(info), cancellationToken);
            return Ok();
        }

        #endregion
    }
}