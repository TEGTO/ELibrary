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
                    .Include(b => b.CoverType)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }
        public override async Task<IEnumerable<Book>> GetPaginatedAsync(LibraryPaginationRequest paginationParams, CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);
            List<Book> paginatedBooks = new List<Book>();

            if (paginationParams is BookPaginationRequest bookPaginationParams)
            {
                var query = queryable
                    .Include(b => b.Author)
                    .Include(b => b.Genre)
                    .Include(b => b.Publisher)
                    .Include(b => b.CoverType)
                    .AsNoTracking();

                if (bookPaginationParams.OnlyInStock)
                {
                    query = query.Where(b => b.StockAmount > 0);
                }
                if (!string.IsNullOrEmpty(bookPaginationParams.ContainsName))
                {
                    query = query.Where(b => b.Name.Contains(bookPaginationParams.ContainsName));
                }
                if (bookPaginationParams.AuthorId.HasValue)
                {
                    query = query.Where(b => b.AuthorId == bookPaginationParams.AuthorId.Value);
                }
                if (bookPaginationParams.GenreId.HasValue)
                {
                    query = query.Where(b => b.GenreId == bookPaginationParams.GenreId.Value);
                }
                if (bookPaginationParams.PublisherId.HasValue)
                {
                    query = query.Where(b => b.PublisherId == bookPaginationParams.PublisherId.Value);
                }
                if (bookPaginationParams.CoverTypeId.HasValue)
                {
                    query = query.Where(b => b.CoverTypeId == bookPaginationParams.CoverTypeId.Value);
                }

                query = query.Where(b => b.PublicationDate >= bookPaginationParams.PublicationFromUTC
                                         && b.PublicationDate <= bookPaginationParams.PublicationToUTC);

                query = query.Where(b => b.Price >= bookPaginationParams.MinPrice
                                         && b.Price <= bookPaginationParams.MaxPrice);

                query = query.Where(b => b.PageAmount >= bookPaginationParams.MinPageAmount
                                         && b.PageAmount <= bookPaginationParams.MaxPageAmount);

                paginatedBooks.AddRange(await query
                    .OrderByDescending(b => b.Id)
                    .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                    .Take(paginationParams.PageSize)
                    .ToListAsync(cancellationToken));
            }
            else
            {
                paginatedBooks.AddRange(await queryable
                                          .Include(b => b.Author)
                                          .Include(b => b.Genre)
                                          .Include(b => b.Publisher)
                                          .Include(b => b.CoverType)
                                          .Where(b => b.Name.Contains(paginationParams.ContainsName))
                                          .OrderByDescending(b => b.Id)
                                          .AsNoTracking()
                                          .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                                          .Take(paginationParams.PageSize)
                                          .ToListAsync(cancellationToken));
            }

            return paginatedBooks;
        }
        public override async Task<Book> CreateAsync(Book book, CancellationToken cancellationToken)
        {
            book = await repository.AddAsync(book, cancellationToken);

            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);

            var entityInDb = await queryable
                                       .Include(b => b.Author)
                                       .Include(b => b.Genre)
                                       .Include(b => b.Publisher)
                                       .Include(b => b.CoverType)
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
                                         .Include(b => b.CoverType)
                                         .FirstAsync(b => b.Id == entityInDb.Id, cancellationToken);
            return entityInDb;
        }
    }
}