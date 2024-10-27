using Microsoft.AspNetCore.Mvc;
using ShopApi.Features.AdvisorFeature.Domain.Dtos;
using ShopApi.Features.AdvisorFeature.Services;

namespace ShopApi.Controllers
{
    [Route("advisor")]
    [ApiController]
    public class AdvisorController : ControllerBase
    {
        private readonly IAdvisorService advisorService;

        public AdvisorController(IAdvisorService advisorService)
        {
            this.advisorService = advisorService;
        }

        [ResponseCache(Duration = 3)]
        [HttpPost]
        public async Task<ActionResult<string>> SendQuery(AdvisorQueryRequest request, CancellationToken cancellationToken)
        {
            var response = await advisorService.SendQueryAsync(request.Query, cancellationToken);
            return Ok(new AdvisorResponse() { Message = response });
        }
    }
}
