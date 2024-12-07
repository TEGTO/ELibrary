using DatabaseControl.Repositories;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using Pagination;

namespace LibraryShopEntities.Repositories.Shop
{
    public class StockBookOrderRepository : IStockBookOrderRepository
    {
        private readonly IDatabaseRepository<ShopDbContext> repository;

        public StockBookOrderRepository(IDatabaseRepository<ShopDbContext> repository)
        {
            this.repository = repository;
        }

        public async Task<StockBookOrder?> GetStockBookOrderByIdAsync(int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<StockBookOrder>(cancellationToken);

            return await queryable
                .AsSplitQuery()
                .AsNoTracking()
                .Include(x => x.Client)
                .Include(x => x.StockBookChanges)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<IEnumerable<StockBookOrder>> GetPaginatedStockBookOrdersAsync(PaginationRequest pagination, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<StockBookOrder>(cancellationToken);

            return await queryable
                .AsSplitQuery()
                .AsNoTracking()
                .Include(x => x.Client)
                .Include(x => x.StockBookChanges)
                .OrderByDescending(b => b.CreatedAt)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);
        }
        public async Task<int> GetStockBookOrderAmountAsync(CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<StockBookOrder>(cancellationToken);
            return await queryable.CountAsync();
        }
        public async Task<StockBookOrder> AddStockBookOrderAsync(StockBookOrder stockBookOrder, CancellationToken cancellationToken)
        {
            return await repository.AddAsync(stockBookOrder, cancellationToken);
        }
    }
}