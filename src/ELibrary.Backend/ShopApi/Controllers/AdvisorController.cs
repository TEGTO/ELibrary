using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using ShopApi.Features.AdvisorFeature.Domain.Dtos;
using ShopApi.Features.AdvisorFeature.Services;

namespace ShopApi.Controllers
{
    [Route("advisor")]
    [ApiController]
    public class AdvisorController : ControllerBase
    {
        private readonly IAdvisorService advisorService;
        private readonly ICacheService cacheService;
        private readonly IMapper mapper;

        public AdvisorController(IAdvisorService advisorService, ICacheService cacheService, IMapper mapper)
        {
            this.advisorService = advisorService;
            this.cacheService = cacheService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<AdvisorResponse?>> SendQuery(AdvisorQueryRequest request, CancellationToken cancellationToken)
        {
            var cacheKey = $"AdvisorResponse_{request.Query}";
            var cachedResponse = cacheService.Get<AdvisorResponse>(cacheKey);

            if (cachedResponse == null)
            {
                var chatRequest = mapper.Map<ChatAdvisorQueryRequest>(request);
                var response = await advisorService.SendQueryAsync(chatRequest, cancellationToken);
                cachedResponse = mapper.Map<AdvisorResponse>(response);

                cacheService.Set(cacheKey, cachedResponse, TimeSpan.FromSeconds(3));
            }

            return Ok(cachedResponse);
        }
    }
}
