using Authentication.Identity;
using AutoMapper;
using Caching.Helpers;
using Caching.Services;
using LibraryApi.Services;
using LibraryShopEntities.Domain.Dtos.SharedRequests;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;
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
        protected readonly ICacheService cacheService;
        protected readonly ICachingHelper cachingHelper;
        protected readonly IMapper mapper;

        protected BaseLibraryEntityController(
            ILibraryEntityService<TEntity> entityService,
            ICacheService cacheService,
            ICachingHelper cachingHelper,
            IMapper mapper
            )
        {
            this.entityService = entityService;
            this.cacheService = cacheService;
            this.cachingHelper = cachingHelper;
            this.mapper = mapper;
        }

        #region Endpoints

        [ResponseCache(Duration = 1)]
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
        [HttpPost("ids")]
        [AllowAnonymous]
        public virtual async Task<ActionResult<IEnumerable<TGetResponse>>> GetByIds(GetByIdsRequest request, CancellationToken cancellationToken)
        {
            var cacheKey = cachingHelper.GetCacheKey($"GetByIds_{typeof(TGetResponse).Name}", HttpContext);
            var cachedResponse = cacheService.Get<List<TGetResponse>>(cacheKey);

            if (cachedResponse == null)
            {
                var entities = await entityService.GetByIdsAsync(request.Ids, cancellationToken);
                cachedResponse = entities.Select(mapper.Map<TGetResponse>).ToList();

                cacheService.Set(cacheKey, cachedResponse, TimeSpan.FromSeconds(1));
            }

            return Ok(cachedResponse);
        }
        [AllowAnonymous]
        [HttpPost("pagination")]
        public virtual async Task<ActionResult<IEnumerable<TGetResponse>>> GetPaginated(TFilterRequest request, CancellationToken cancellationToken)
        {
            var cacheKey = cachingHelper.GetCacheKey($"GetPaginated_{typeof(TGetResponse).Name}", HttpContext);
            var cachedResponse = cacheService.Get<List<TGetResponse>>(cacheKey);

            if (cachedResponse == null)
            {
                var entities = await entityService.GetPaginatedAsync(request, cancellationToken);
                cachedResponse = entities.Select(mapper.Map<TGetResponse>).ToList();

                cacheService.Set(cacheKey, cachedResponse, TimeSpan.FromSeconds(10));
            }

            return Ok(cachedResponse);
        }
        [AllowAnonymous]
        [HttpPost("amount")]
        public virtual async Task<ActionResult<int>> GetItemTotalAmount(TFilterRequest request, CancellationToken cancellationToken)
        {
            var cacheKey = cachingHelper.GetCacheKey($"GetItemTotalAmount_{typeof(TGetResponse).Name}", HttpContext);
            var cachedResponse = cacheService.Get<int?>(cacheKey);

            if (cachedResponse == null)
            {
                var amount = await entityService.GetItemTotalAmountAsync(request, cancellationToken);
                cachedResponse = amount;

                cacheService.Set(cacheKey, cachedResponse, TimeSpan.FromSeconds(10));
            }

            return Ok(cachedResponse);
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
            await entityService.DeleteAsync(id, cancellationToken);
            return Ok();
        }

        #endregion
    }
}