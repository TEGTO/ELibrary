﻿using LibraryApi.Domain.Dtos;
using LibraryShopEntities.Domain.Entities.Library;

namespace LibraryApi.Services
{
    public interface ILibraryEntityService<TEntity> where TEntity : BaseLibraryEntity
    {
        public Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken);
        public Task DeleteByIdAsync(int id, CancellationToken cancellationToken);
        public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<int> GetItemTotalAmountAsync(LibraryFilterRequest req, CancellationToken cancellationToken);
        public Task<IEnumerable<TEntity>> GetPaginatedAsync(LibraryFilterRequest req, CancellationToken cancellationToken);
        public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}