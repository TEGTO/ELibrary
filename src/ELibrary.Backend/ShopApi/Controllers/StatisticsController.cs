using Authentication.Identity;
using AutoMapper;
using Caching.Helpers;
using Caching.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Features.StatisticsFeature.Domain.Dtos;
using ShopApi.Features.StatisticsFeature.Domain.Models;
using ShopApi.Features.StatisticsFeature.Services;

namespace ShopApi.Controllers
{
    [Authorize(Policy = Policy.REQUIRE_MANAGER_ROLE)]
    [Route("statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IStatisticsService statisticsService;
        private readonly ICacheService cacheService;
        private readonly ICachingHelper cachingHelper;

        public StatisticsController(
            IMapper mapper,
            IStatisticsService statisticsService,
            ICacheService cacheService,
            ICachingHelper cachingHelper
            )
        {
            this.mapper = mapper;
            this.statisticsService = statisticsService;
            this.cacheService = cacheService;
            this.cachingHelper = cachingHelper;
        }

        #region EndPoints

        [HttpPost]
        public async Task<ActionResult<ShopStatisticsResponse>> GetShopStatistics(GetShopStatisticsRequest request, CancellationToken cancellationToken)
        {
            var cacheKey = cachingHelper.GetCacheKey(
                $"GetShopStatistics_{request?.FromUTC ?? DateTime.MinValue}_{request?.ToUTC ?? DateTime.MinValue}_{request.IncludeBooks?.Count() ?? 0}",
                HttpContext);
            var cachedResponse = cacheService.Get<ShopStatisticsResponse>(cacheKey);

            if (cachedResponse == null)
            {
                var getStatistics = mapper.Map<GetShopStatisticsFilter>(request);
                var response = await statisticsService.GetStatisticsAsync(getStatistics, cancellationToken);
                cachedResponse = mapper.Map<ShopStatisticsResponse>(response);

                cacheService.Set(cacheKey, cachedResponse, TimeSpan.FromSeconds(10));
            }

            return Ok(cachedResponse);
        }

        #endregion
    }
}