using AutoMapper;
using LibraryShopEntities.Domain.Dtos;
using LibraryShopEntities.Domain.Dtos.Library;
using LibraryShopEntities.Domain.Dtos.Library.CoverType;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryShopEntities.Controllers
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
