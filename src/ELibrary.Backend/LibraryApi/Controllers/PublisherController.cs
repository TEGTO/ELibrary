using AutoMapper;
using Caching.Helpers;
using Caching.Services;
using LibraryApi.Domain.Dto.Publisher;
using LibraryApi.Services;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
