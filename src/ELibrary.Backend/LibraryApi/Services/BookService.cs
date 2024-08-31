using LibraryApi.Data;
using LibraryApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;

namespace LibraryApi.Services
{
    public class BookService : LibraryEntityService<Book>
    {
        public BookService(IDatabaseRepository<LibraryDbContext> repository) : base(repository)
        {
        }

        public override async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            using (var dbContext = await CreateDbContextAsync(cancellationToken))
            {
                return await dbContext.Set<Book>()
                                      .Include(b => b.Author)
                                      .Include(b => b.Genre)
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
            }
        }
        public override async Task<IEnumerable<Book>> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            using (var dbContext = await CreateDbContextAsync(cancellationToken))
            {
                var paginatedBooks = await dbContext.Set<Book>()
                                                    .Include(b => b.Author)
                                                    .Include(b => b.Genre)
                                                    .OrderByDescending(b => b.Id)
                                                    .AsNoTracking()
                                                    .Skip((pageNumber - 1) * pageSize)
                                                    .Take(pageSize)
                                                    .ToListAsync(cancellationToken);

                return paginatedBooks;
            }
        }
        public override async Task<Book> CreateAsync(Book book, CancellationToken cancellationToken)
        {
            using (var dbContext = await CreateDbContextAsync(cancellationToken))
            {
                if (book.Author == null || book.Genre == null)
                {
                    book.Author = dbContext.Authors.First(x => x.Id == book.AuthorId);
                    book.Genre = dbContext.Genres.First(x => x.Id == book.GenreId);
                }

                await dbContext.Set<Book>().AddAsync(book, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
                return book;
            }
        }
        public override async Task UpdateAsync(Book entity, CancellationToken cancellationToken)
        {
            using (var dbContext = await CreateDbContextAsync(cancellationToken))
            {
                var entityInDb = await dbContext.Set<Book>()
                                                 .Include(b => b.Author)
                                                 .Include(b => b.Genre)
                                                 .FirstOrDefaultAsync(b => b.Id == entity.Id, cancellationToken);

                if (entityInDb != null)
                {
                    entityInDb.Copy(entity);

                    if (entityInDb.AuthorId != entity.AuthorId)
                    {
                        var author = await dbContext.Set<Author>().FirstOrDefaultAsync(a => a.Id == entity.AuthorId, cancellationToken);
                        if (author != null)
                        {
                            entityInDb.AuthorId = entity.AuthorId;
                            entityInDb.Author = author;
                        }
                    }

                    if (entityInDb.GenreId != entity.GenreId)
                    {
                        var genre = await dbContext.Set<Genre>().FirstOrDefaultAsync(g => g.Id == entity.GenreId, cancellationToken);
                        if (genre != null)
                        {
                            entityInDb.GenreId = entity.GenreId;
                            entityInDb.Genre = genre;
                        }
                    }

                    dbContext.Set<Book>().Update(entityInDb);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}