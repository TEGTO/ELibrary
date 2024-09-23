using Authentication.Identity;
using AutoMapper;
using LibraryApi.Domain.Dtos;
using LibraryApi.Services;
using LibraryShopEntities.Domain.Entities.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    [ApiController]
    public abstract class BaseLibraryEntityController<
        TEntity,
        TGetResponse,
        TCreateRequest,
        TCreateResponse,
        TUpdateRequest,
        TUpdateResponse,
        TFilterRequest> : ControllerBase
     where TEntity : BaseLibraryEntity where TFilterRequest : LibraryFilterRequest
    {
        protected readonly ILibraryEntityService<TEntity> entityService;
        protected readonly IMapper mapper;

        protected BaseLibraryEntityController(ILibraryEntityService<TEntity> entityService, IMapper mapper)
        {
            this.entityService = entityService;
            this.mapper = mapper;
        }

        #region Endpoints

        [HttpGet("{id}")]
        [AllowAnonymous]
        public virtual async Task<ActionResult<TGetResponse>> GetById(int id, CancellationToken cancellationToken)
        {
            var entity = await entityService.GetByIdAsync(id, cancellationToken);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<TGetResponse>(entity));
        }
        [AllowAnonymous]
        [HttpPost("pagination")]
        public virtual async Task<ActionResult<IEnumerable<TGetResponse>>> GetPaginated(TFilterRequest request, CancellationToken cancellationToken)
        {
            var entities = await entityService.GetPaginatedAsync(request, cancellationToken);
            return Ok(entities.Select(mapper.Map<TGetResponse>));
        }
        [AllowAnonymous]
        [HttpPost("amount")]
        public virtual async Task<ActionResult<int>> GetItemTotalAmount(TFilterRequest request, CancellationToken cancellationToken)
        {
            var amount = await entityService.GetItemTotalAmountAsync(request, cancellationToken);
            return Ok(amount);
        }

        #endregion

        #region Manager Endpoints

        [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
        [HttpPost]
        public virtual async Task<ActionResult<TCreateResponse>> Create(TCreateRequest request, CancellationToken cancellationToken)
        {
            var entityToCreate = mapper.Map<TEntity>(request);
            var entityResponse = await entityService.CreateAsync(entityToCreate, cancellationToken);

            var response = mapper.Map<TCreateResponse>(entityResponse);

            return Created(string.Empty, response);
        }
        [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
        [HttpPut]
        public virtual async Task<ActionResult<TUpdateResponse>> Update(TUpdateRequest request, CancellationToken cancellationToken)
        {
            var entityToUpdate = mapper.Map<TEntity>(request);
            var entityResponse = await entityService.UpdateAsync(entityToUpdate, cancellationToken);

            var response = mapper.Map<TUpdateResponse>(entityResponse);

            return Ok(response);
        }
        [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteById(int id, CancellationToken cancellationToken)
        {
            await entityService.DeleteByIdAsync(id, cancellationToken);
            return Ok();
        }

        #endregion
    }
}