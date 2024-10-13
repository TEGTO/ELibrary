using LibraryApi.Domain.Dtos;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;

namespace LibraryApi.Services
{
    public class AuthorService : LibraryEntityService<Author>
    {
        public AuthorService(IDatabaseRepository<LibraryShopDbContext> repository) : base(repository)
        {
        }

        public override async Task<IEnumerable<Author>> GetPaginatedAsync(LibraryFilterRequest req, CancellationToken cancellationToken)
        {
            var list = new List<Author>();
            var queryable = await repository.GetQueryableAsync<Author>(cancellationToken);

            list.AddRange(await queryable
                  .AsNoTracking()
                  .Where(x => (x.Name + " " + x.LastName).Contains(req.ContainsName))
                  .OrderByDescending(b => b.Id)
                  .Skip((req.PageNumber - 1) * req.PageSize)
                  .Take(req.PageSize)
                  .ToListAsync(cancellationToken));

            return list;
        }
        public override async Task<int> GetItemTotalAmountAsync(LibraryFilterRequest req, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Author>(cancellationToken);

            queryable = queryable
                .AsNoTracking()
                .Where(x => (x.Name + " " + x.LastName).Contains(req.ContainsName));

            return await queryable.CountAsync(cancellationToken);
        }
    }
}
