using AutoMapper;
using LibraryShopEntities.Domain.Dtos;
using LibraryShopEntities.Domain.Dtos.Library.Publisher;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryShopEntities.Controllers
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
        LibraryPaginationRequest>
    {
        public PublisherController(ILibraryEntityService<Publisher> entityService, IMapper mapper) : base(entityService, mapper)
        {
        }
    }
}
