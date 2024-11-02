using Authentication.Identity;
using AutoMapper;
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

        public StatisticsController(IMapper mapper, IStatisticsService statisticsService)
        {
            this.mapper = mapper;
            this.statisticsService = statisticsService;
        }

        #region EndPoints

        [ResponseCache(Duration = 10)]
        [HttpPost]
        public async Task<ActionResult<ShopStatisticsResponse>> GetShopStatistics(GetShopStatisticsRequest request, CancellationToken cancellationToken)
        {
            var getStatistics = mapper.Map<GetShopStatistics>(request);

            var statistics = await statisticsService.GetStatisticsAsync(getStatistics, cancellationToken);

            return Ok(mapper.Map<ShopStatisticsResponse>(statistics));
        }

        #endregion
    }
}