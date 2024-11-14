using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using ShopApi.Features.AdvisorFeature.Domain.Dtos;
using ShopApi.Features.AdvisorFeature.Services;

namespace ShopApi.Controllers
{
    [Route("advisor")]
    [ApiController]
    public class AdvisorController : ControllerBase
    {
        private readonly IAdvisorService advisorService;
        private readonly IMapper mapper;

        public AdvisorController(IAdvisorService advisorService, IMapper mapper)
        {
            this.advisorService = advisorService;
            this.mapper = mapper;
        }

        [HttpPost]
        [OutputCache(PolicyName = "AdvisorPolicy")]
        public async Task<ActionResult<AdvisorResponse?>> SendQuery(AdvisorQueryRequest request, CancellationToken cancellationToken)
        {
            var chatRequest = mapper.Map<ChatAdvisorQueryRequest>(request);
            var response = await advisorService.SendQueryAsync(chatRequest, cancellationToken);

            return Ok(mapper.Map<AdvisorResponse>(response));
        }
    }
}
