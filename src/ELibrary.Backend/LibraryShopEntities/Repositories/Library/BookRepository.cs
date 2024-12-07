using DatabaseControl.Repositories;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Filters;
using Microsoft.EntityFrameworkCore;

namespace LibraryShopEntities.Repositories.Library
{
    public sealed class BookRepository : LibraryEntityRepository<Book>, IBookRepository
    {
        public BookRepository(IDatabaseRepository<LibraryDbContext> repository) : base(repository)
        {
        }

        public override async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);

            return await
                 IncludeBookEntities(queryable)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }
        public override async Task<IEnumerable<Book>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);

            return await
                 IncludeBookEntities(queryable)
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(cancellationToken);
        }
        public async Task<IEnumerable<BookPopularity>> GetPopularitiesByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<BookPopularity>(cancellationToken);

            var popularities = await queryable.Where(bp => ids.Contains(bp.BookId)).ToListAsync(cancellationToken);

            return popularities;
        }
        public async Task UpdatePopularityRangeAsync(IEnumerable<BookPopularity> popularities, CancellationToken cancellationToken)
        {
            await repository.UpdateRangeAsync(popularities, cancellationToken);
        }
        public override async Task<IEnumerable<Book>> GetPaginatedAsync(LibraryFilterRequest req, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);
            List<Book> paginatedBooks = new List<Book>();

            var query = IncludeBookEntities(queryable).AsNoTracking();

            query = ApplyFilter(query, req);

            var sortedBooks = await ApplySorting(query, req).ToListAsync(cancellationToken);

            paginatedBooks.AddRange(sortedBooks
                  .Skip((req.PageNumber - 1) * req.PageSize)
                  .Take(req.PageSize));

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
            return await GetByIdAsync(book.Id, cancellationToken) ?? throw new InvalidOperationException("Can not get created book!");
        }
        public override async Task<Book> UpdateAsync(Book book, CancellationToken cancellationToken)
        {
            await repository.UpdateAsync(book, cancellationToken);
            return await GetByIdAsync(book.Id, cancellationToken) ?? throw new InvalidOperationException("Can not get updated book!");
        }
        protected override IQueryable<Book> ApplyFilter(IQueryable<Book> query, LibraryFilterRequest req)
        {
            if (req is BookFilterRequest bookFilter)
            {
                query = bookFilter.OnlyInStock == true ? query.Where(b => b.StockAmount > 0) : query;
                query = !string.IsNullOrEmpty(bookFilter.ContainsName) ? query.Where(b => b.Name.Contains(bookFilter.ContainsName)) : query;
                query = bookFilter.AuthorId.HasValue ? query.Where(b => b.AuthorId == bookFilter.AuthorId.Value) : query;
                query = bookFilter.GenreId.HasValue ? query.Where(b => b.GenreId == bookFilter.GenreId.Value) : query;
                query = bookFilter.PublisherId.HasValue ? query.Where(b => b.PublisherId == bookFilter.PublisherId.Value) : query;
                query = bookFilter.CoverType.HasValue && bookFilter.CoverType != CoverType.Any ? query.Where(b => b.CoverType == bookFilter.CoverType.Value) : query;
                query = bookFilter.PublicationFrom.HasValue ? query.Where(b => b.PublicationDate >= bookFilter.PublicationFrom) : query;
                query = bookFilter.PublicationTo.HasValue ? query.Where(b => b.PublicationDate <= bookFilter.PublicationTo) : query;
                query = bookFilter.MinPrice.HasValue ? query.Where(b => b.Price >= bookFilter.MinPrice) : query;
                query = bookFilter.MaxPrice.HasValue ? query.Where(b => b.Price <= bookFilter.MaxPrice) : query;
                query = bookFilter.MinPageAmount.HasValue ? query.Where(b => b.PageAmount >= bookFilter.MinPageAmount) : query;
                query = bookFilter.MaxPageAmount.HasValue ? query.Where(b => b.PageAmount <= bookFilter.MaxPageAmount) : query;

                return query;
            }
            return query.Where(b => b.Name.Contains(req.ContainsName));
        }
        private static IQueryable<Book> ApplySorting(IQueryable<Book> query, LibraryFilterRequest req)
        {
            if (req is BookFilterRequest bookFilter)
            {
                if (!bookFilter.Sorting.HasValue)
                {
                    bookFilter.Sorting = BookSorting.MostPopular;
                }

                return bookFilter.Sorting switch
                {
                    BookSorting.MostPopular => query.OrderByDescending(b => b.StockAmount > 0)
                                                    .ThenByDescending(b => b.BookPopularity != null ? b.BookPopularity.Popularity : 0),
                    BookSorting.LeastPopular => query.OrderByDescending(b => b.StockAmount > 0)
                                                     .ThenBy(b => b.BookPopularity != null ? b.BookPopularity.Popularity : 0),
                    _ => query.OrderByDescending(b => b.StockAmount > 0).ThenByDescending(b => b.Id)
                };
            }

            return query
                .OrderByDescending(b => b.StockAmount > 0)
                .ThenByDescending(b => b.Id);
        }
        private static IQueryable<Book> IncludeBookEntities(IQueryable<Book> queryable)
        {
            return queryable.Include(b => b.Author).Include(b => b.Genre).Include(b => b.Publisher);
        }
    }
}