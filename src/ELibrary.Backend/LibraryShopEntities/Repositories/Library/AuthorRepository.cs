using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;
using Shared.Repositories;

namespace LibraryShopEntities.Repositories.Library
{
    public class AuthorRepository : LibraryEntityRepository<Author>
    {
        public AuthorRepository(IDatabaseRepository<LibraryDbContext> repository) : base(repository)
        {
        }

        protected override IQueryable<Author> ApplyFilter(IQueryable<Author> query, LibraryFilterRequest req)
        {
            if (!string.IsNullOrEmpty(req.ContainsName))
            {
                query = query.Where(x => (x.Name + " " + x.LastName).Contains(req.ContainsName));
            }
            return query;
        }
    }
}
