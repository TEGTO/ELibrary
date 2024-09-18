using LibraryApi.Domain.Dtos;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;

namespace LibraryApi.Services
{
    public class BookService : LibraryEntityService<Book>
    {
        public BookService(IDatabaseRepository<LibraryShopDbContext> repository) : base(repository)
        {
        }

        public override async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);

            return await queryable
                    .Include(b => b.Author)
                    .Include(b => b.Genre)
                    .Include(b => b.Publisher)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }
        public override async Task<IEnumerable<Book>> GetPaginatedAsync(LibraryFilterRequest req, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);
            List<Book> paginatedBooks = new List<Book>();

            var query = queryable
                 .Include(b => b.Author)
                 .Include(b => b.Genre)
                 .Include(b => b.Publisher)
                 .AsNoTracking();

            query = ApplyFilter(query, req);

            paginatedBooks.AddRange(await query
                  .OrderByDescending(b => b.Id)
                  .Skip((req.PageNumber - 1) * req.PageSize)
                  .Take(req.PageSize)
                  .ToListAsync(cancellationToken));

            return paginatedBooks;
        }
        public override async Task<int> GetItemTotalAmountAsync(LibraryFilterRequest req, CancellationToken cancellationToken)
        {
            var query = (await repository.GetQueryableAsync<Book>(cancellationToken)).AsNoTracking();

            query = ApplyFilter(query, req);

            return await query.CountAsync(cancellationToken);
        }
        public override async Task<Book> CreateAsync(Book book, CancellationToken cancellationToken)
        {
            book = await repository.AddAsync(book, cancellationToken);

            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);

            var entityInDb = await queryable
                                       .Include(b => b.Author)
                                       .Include(b => b.Genre)
                                       .Include(b => b.Publisher)
                                       .FirstAsync(b => b.Id == book.Id, cancellationToken);
            return entityInDb;
        }
        public override async Task<Book> UpdateAsync(Book entity, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);

            var entityInDb = await queryable.FirstAsync(b => b.Id == entity.Id, cancellationToken);

            entityInDb.Copy(entity);

            await repository.UpdateAsync(entityInDb, cancellationToken);

            entityInDb = await queryable
                                         .Include(b => b.Author)
                                         .Include(b => b.Genre)
                                         .Include(b => b.Publisher)
                                         .FirstAsync(b => b.Id == entityInDb.Id, cancellationToken);
            return entityInDb;
        }

        private IQueryable<Book> ApplyFilter(IQueryable<Book> query, LibraryFilterRequest req)
        {
            if (req is BookFilterRequest bookFilter)
            {
                if (bookFilter.OnlyInStock.HasValue && bookFilter.OnlyInStock.Value)
                {
                    query = query.Where(b => b.StockAmount > 0);
                }
                if (!string.IsNullOrEmpty(bookFilter.ContainsName))
                {
                    query = query.Where(b => b.Name.Contains(bookFilter.ContainsName));
                }
                if (bookFilter.AuthorId.HasValue)
                {
                    query = query.Where(b => b.AuthorId == bookFilter.AuthorId.Value);
                }
                if (bookFilter.GenreId.HasValue)
                {
                    query = query.Where(b => b.GenreId == bookFilter.GenreId.Value);
                }
                if (bookFilter.PublisherId.HasValue)
                {
                    query = query.Where(b => b.PublisherId == bookFilter.PublisherId.Value);
                }
                if (bookFilter.CoverType.HasValue && bookFilter.CoverType != CoverType.Any)
                {
                    query = query.Where(b => b.CoverType == bookFilter.CoverType);
                }
                if (bookFilter.PublicationFromUTC.HasValue)
                {
                    query = query.Where(b => b.PublicationDate >= bookFilter.PublicationFromUTC);
                }
                if (bookFilter.PublicationToUTC.HasValue)
                {
                    query = query.Where(b => b.PublicationDate <= bookFilter.PublicationToUTC);
                }
                if (bookFilter.MinPrice.HasValue)
                {
                    query = query.Where(b => b.Price >= bookFilter.MinPrice);
                }
                if (bookFilter.MaxPrice.HasValue)
                {
                    query = query.Where(b => b.Price <= bookFilter.MaxPrice);
                }
                if (bookFilter.MinPageAmount.HasValue)
                {
                    query = query.Where(b => b.PageAmount >= bookFilter.MinPageAmount);
                }
                if (bookFilter.MaxPageAmount.HasValue)
                {
                    query = query.Where(b => b.PageAmount <= bookFilter.MaxPageAmount);
                }
                return query;
            }
            else
            {
                return query.Where(b => b.Name.Contains(req.ContainsName));
            }
        }
    }
}