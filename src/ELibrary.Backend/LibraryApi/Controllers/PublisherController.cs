using AutoMapper;
using LibraryApi.Domain.Dtos;
using LibraryApi.Domain.Dtos.Library.Publisher;
using LibraryApi.Services;
using LibraryShopEntities.Domain.Dtos.Library.Publisher;
using LibraryShopEntities.Domain.Entities.Library;
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
        LibraryPaginationRequest>
    {
        public PublisherController(ILibraryEntityService<Publisher> entityService, IMapper mapper) : base(entityService, mapper)
        {
        }
    }
}
