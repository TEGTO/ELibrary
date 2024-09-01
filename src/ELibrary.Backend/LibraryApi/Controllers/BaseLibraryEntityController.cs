using AutoMapper;
using LibraryApi.Domain.Dto;
using LibraryApi.Domain.Entities;
using LibraryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    [ApiController]
    public abstract class BaseLibraryEntityController<TEntity, TGetResponse, TCreateRequest, TCreateResponse, TUpdateRequest, TUpdateResponse> : ControllerBase
     where TEntity : BaseEntity
    {
        protected readonly ILibraryEntityService<TEntity> entityService;
        protected readonly IMapper mapper;

        protected BaseLibraryEntityController(ILibraryEntityService<TEntity> entityService, IMapper mapper)
        {
            this.entityService = entityService;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TGetResponse>> GetById(int id, CancellationToken cancellationToken)
        {
            var entity = await entityService.GetByIdAsync(id, cancellationToken);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<TGetResponse>(entity));
        }

        [HttpPost("pagination")]
        public async Task<ActionResult<IEnumerable<TGetResponse>>> GetPaginated(PaginatedRequest request, CancellationToken cancellationToken)
        {
            var entities = await entityService.GetPaginatedAsync(request.PageNumber, request.PageSize, cancellationToken);
            return Ok(entities.Select(mapper.Map<TGetResponse>));
        }

        [HttpGet("amount")]
        public async Task<ActionResult<int>> GetItemTotalAmount(CancellationToken cancellationToken)
        {
            var amount = await entityService.GetItemTotalAmountAsync(cancellationToken);
            return Ok(amount);
        }

        [HttpPost]
        public async Task<ActionResult<TCreateResponse>> Create(TCreateRequest request, CancellationToken cancellationToken)
        {
            var entityToCreate = mapper.Map<TEntity>(request);
            var entityResponse = await entityService.CreateAsync(entityToCreate, cancellationToken);

            var response = mapper.Map<TCreateResponse>(entityResponse);

            return Created(string.Empty, response);
        }

        [HttpPut]
        public async Task<ActionResult<TUpdateResponse>> Update(TUpdateRequest request, CancellationToken cancellationToken)
        {
            var entityToUpdate = mapper.Map<TEntity>(request);
            var entityResponse = await entityService.UpdateAsync(entityToUpdate, cancellationToken);

            var response = mapper.Map<TUpdateResponse>(entityResponse);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id, CancellationToken cancellationToken)
        {
            await entityService.DeleteByIdAsync(id, cancellationToken);
            return Ok();
        }
    }
}