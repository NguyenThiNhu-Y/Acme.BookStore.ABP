using Acme.BookStore.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Acme.BookStore.Books
{
    public class MongoBookRepository : MongoDbRepository<BookStoreMongoDbContext, Book, Guid>, IBookRepository
    {
        public MongoBookRepository(IMongoDbContextProvider<BookStoreMongoDbContext> dbContext) : base(dbContext)
        {

        }

        public async Task<long> GetCountAsync(string filterText = null, string name = null, Guid? IdAuthor = null, Guid? IdCategory = null, CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, name,IdAuthor, IdCategory);
            return await query.As<IMongoQueryable<Book>>().LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<Book>> GetListAsync( string filterText = null, string name = null, Guid? IdAuthor = null, Guid? IdCategory = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, name, IdAuthor, IdCategory);
            return await query.As<IMongoQueryable<Book>>()
                .OrderByDescending(x => x.CreationTime)
                .PageBy<Book, IMongoQueryable<Book>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Book> ApplyFilter(
            IQueryable<Book> query,
            
            string filterText, string name = null, Guid? idAuthor = null, Guid? idCategory = null)
        {
            var dbContext = GetDbContextAsync();
            var getDashboards = query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Name.ToLower().Contains(filterText.ToLower()))
                .WhereIf(idAuthor.HasValue, e => e.IdAuthor == idAuthor)
                .WhereIf(idCategory.HasValue, e =>e.Type == idCategory)
                ;
            return getDashboards;
        }
    }
}
