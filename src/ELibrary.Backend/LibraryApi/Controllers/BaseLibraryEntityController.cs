﻿using AutoMapper;
using LibraryApi.Domain.Entities;
using LibraryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    [ApiController]
    public abstract class BaseLibraryEntityController<TEntity, TGetResponse, TCreateRequest, TCreateResponse, TUpdateRequest> : ControllerBase
     where TEntity : BaseEntity
    {
        protected readonly ILibraryEntityService<TEntity> entityService;
        protected readonly IMapper mapper;

        protected BaseLibraryEntityController(ILibraryEntityService<TEntity> entityService, IMapper mapper)
        {
            this.entityService = entityService;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TGetResponse>> GetById(string id, CancellationToken cancellationToken)
        {
            var entity = await entityService.GetByIdAsync(id, cancellationToken);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<TGetResponse>(entity));
        }

        [HttpPost]
        public async Task<ActionResult<TCreateResponse>> Create(TCreateRequest request, CancellationToken cancellationToken)
        {
            var entityToCreate = mapper.Map<TEntity>(request);
            var entityResponse = await entityService.CreateAsync(entityToCreate, cancellationToken);

            var response = mapper.Map<TCreateResponse>(entityResponse);

            return Created(string.Empty, response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(TUpdateRequest request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<TEntity>(request);
            await entityService.UpdateAsync(entity, cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(string id, CancellationToken cancellationToken)
        {
            await entityService.DeleteByIdAsync(id, cancellationToken);
            return Ok();
        }
    }
}