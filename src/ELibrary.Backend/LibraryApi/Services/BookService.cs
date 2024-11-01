using LibraryApi.Domain.Dtos;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Domain.Entities.Shop;
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

            return await
                IncludeBookEnities(queryable)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }
        public virtual async Task<IEnumerable<Book>> GetByIdsAsync(List<int> ids, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);

            return await
                IncludeBookEnities(queryable)
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(cancellationToken);
        }
        public override async Task<IEnumerable<Book>> GetPaginatedAsync(LibraryFilterRequest req, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);
            List<Book> paginatedBooks = new List<Book>();

            var query = IncludeBookEnities(queryable).AsNoTracking();

            query = ApplyFilter(query, req);

            var sortedBooks = await ApplySortingAsync(query, req, cancellationToken);

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

            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);

            var entityInDb = await IncludeBookEnities(queryable).FirstAsync(b => b.Id == book.Id, cancellationToken);
            return entityInDb;
        }
        public override async Task<Book> UpdateAsync(Book entity, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);

            var entityInDb = await queryable.FirstAsync(b => b.Id == entity.Id, cancellationToken);

            entityInDb.Copy(entity);

            await repository.UpdateAsync(entityInDb, cancellationToken);

            entityInDb = await IncludeBookEnities(queryable).FirstAsync(b => b.Id == entityInDb.Id, cancellationToken);
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
                if (bookFilter.PublicationFrom.HasValue)
                {
                    query = query.Where(b => b.PublicationDate >= bookFilter.PublicationFrom);
                }
                if (bookFilter.PublicationTo.HasValue)
                {
                    query = query.Where(b => b.PublicationDate <= bookFilter.PublicationTo);
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
            return query
                .Where(b => b.Name.Contains(req.ContainsName));
        }

        private async Task<IEnumerable<Book>> ApplySortingAsync(IQueryable<Book> query, LibraryFilterRequest req, CancellationToken cancellationToken)
        {
            if (req is BookFilterRequest bookFilter)
            {
                if (!bookFilter.Sorting.HasValue)
                {
                    bookFilter.Sorting = BookSorting.MostPopular;
                }
                var books = await query
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var bookIds = books.Select(x => x.Id);

                var orderBooksCountQuery = await (await repository.GetQueryableAsync<Order>(cancellationToken))
                    .SelectMany(o => o.OrderBooks)
                    .Where(ob => bookIds.Contains(ob.BookId))
                    .GroupBy(ob => ob.BookId)
                    .Select(g => new { BookId = g.Key, OrderCount = g.Count() })
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var orderCountDict = orderBooksCountQuery.ToDictionary(x => x.BookId, x => x.OrderCount);

                var bookToSort = books
                    .Select(book => new
                    {
                        Book = book,
                        OrderCount = orderCountDict.ContainsKey(book.Id) ? orderCountDict[book.Id] : 0
                    });

                switch (bookFilter.Sorting.Value)
                {
                    case BookSorting.MostPopular:
                        return bookToSort
                              .OrderByDescending(b => b.Book.StockAmount > 0)
                              .ThenByDescending(b => b.OrderCount)
                              .Select(b => b.Book)
                              .ToList();
                    case BookSorting.LeastPopular:
                        return bookToSort
                              .OrderByDescending(b => b.Book.StockAmount > 0)
                              .ThenBy(b => b.OrderCount)
                              .Select(b => b.Book)
                              .ToList();
                    default:
                        break;
                }
            }

            return query
                .Where(b => b.Name.Contains(req.ContainsName))
                .OrderByDescending(b => b.StockAmount > 0)
                .ThenByDescending(b => b.Id);
        }
        private IQueryable<Book> IncludeBookEnities(IQueryable<Book> queryable)
        {
            return queryable
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.Publisher);
        }
    }
}