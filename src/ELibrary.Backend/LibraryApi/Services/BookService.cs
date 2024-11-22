using LibraryShopEntities.Domain.Entities.Library;
using LibraryShopEntities.Repositories.Library;

namespace LibraryApi.Services
{
    public class BookService : LibraryEntityService<Book>, IBookService
    {
        public BookService(IBookRepository repository) : base(repository)
        {
        }

        public async Task RaisePopularityAsync(IEnumerable<int> ids, CancellationToken cancellationToken)
        {
            if (ids.Any())
            {
                var existingPopularities = (await ((IBookRepository)repository)
                    .GetPopularitiesByIdsAsync(ids, cancellationToken))
                    .ToDictionary(bp => bp.BookId);

                var popularitiesToUpdate = new List<BookPopularity>();

                foreach (var id in ids)
                {
                    if (existingPopularities.TryGetValue(id, out var popularity))
                    {
                        popularity.Popularity++;
                    }
                    else
                    {
                        popularitiesToUpdate.Add(new BookPopularity { BookId = id, Popularity = 1 });
                    }
                }

                popularitiesToUpdate.AddRange(existingPopularities.Values);

                await ((IBookRepository)repository).UpdatePopularityRangeAsync(popularitiesToUpdate, cancellationToken);
            }
        }
        public async Task ChangeBookStockAmount(Dictionary<int, int> changeRequests, CancellationToken cancellationToken)
        {
            if (changeRequests.Any())
            {
                var bookIds = changeRequests.Keys.ToList();

                var books = await repository.GetByIdsAsync(bookIds, cancellationToken);

                foreach (var book in books)
                {
                    if (changeRequests.TryGetValue(book.Id, out var changeAmount))
                    {
                        book.StockAmount += changeAmount;
                    }
                }

                await repository.UpdateRangeAsync(books.ToArray(), cancellationToken);
            }
        }
    }
}