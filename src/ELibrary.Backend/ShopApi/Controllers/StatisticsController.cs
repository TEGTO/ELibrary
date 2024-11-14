using Authentication.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
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

        public StatisticsController(
            IMapper mapper,
            IStatisticsService statisticsService
            )
        {
            this.mapper = mapper;
            this.statisticsService = statisticsService;
        }

        #region EndPoints

        [HttpPost]
        [OutputCache(PolicyName = "StatisticsPolicy")]
        public async Task<ActionResult<ShopStatisticsResponse>> GetShopStatistics(GetShopStatisticsRequest request, CancellationToken cancellationToken)
        {
            var getStatistics = mapper.Map<GetShopStatisticsFilter>(request);
            var response = await statisticsService.GetStatisticsAsync(getStatistics, cancellationToken);

            return Ok(mapper.Map<ShopStatisticsResponse>(response));
        }

        #endregion
    }
}