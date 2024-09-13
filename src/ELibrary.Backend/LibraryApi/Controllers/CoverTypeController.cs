using AutoMapper;
using LibraryApi.Domain.Dtos;
using LibraryApi.Domain.Dtos.Library.CoverType;
using LibraryApi.Services;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Entities.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    [Route("covertype")]
    [ApiController]
    public class CoverTypeController : BaseLibraryEntityController<
        CoverType,
        CoverTypeResponse,
        CreateCoverTypeRequest,
        CoverTypeResponse,
        UpdateCoverTypeRequest,
        CoverTypeResponse,
        LibraryPaginationRequest>
    {
        public CoverTypeController(ILibraryEntityService<CoverType> entityService, IMapper mapper) : base(entityService, mapper)
        {
        }
    }
}
