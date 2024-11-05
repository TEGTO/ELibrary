using AutoMapper;
using LibraryApi.Domain.Dto.Publisher;
using LibraryApi.Domain.Dtos;
using LibraryApi.Services;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Helpers;
using Shared.Services;

namespace LibraryApi.Controllers
{
    [Authorize]
    [Route("publisher")]
    [ApiController]
    public class PublisherController : BaseLibraryEntityController<
        Publisher,
        PublisherResponse,
        CreatePublisherRequest,
        PublisherResponse,
        UpdatePublisherRequest,
        PublisherResponse,
        LibraryFilterRequest>
    {
        public PublisherController(
            ILibraryEntityService<Publisher> entityService,
            ICacheService cacheService,
            ICachingHelper cachingHelper,
            IMapper mapper
            ) : base(entityService, cacheService, cachingHelper, mapper)
        {
        }
    }
}
