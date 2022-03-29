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

namespace Acme.BookStore.Authors
{
    public class MongoAuthorRepository : MongoDbRepository<BookStoreMongoDbContext, Author, Guid>, IAuthorRepository
    {
        public MongoAuthorRepository(IMongoDbContextProvider<BookStoreMongoDbContext> dbContext) : base(dbContext)
        {

        }
        public async Task<Author> FindByNameAsync(string name)
        {
            var queryable = await GetMongoQueryableAsync();
            return await queryable.Where(author => author.Name == name).FirstOrDefaultAsync();
        }

        public async Task<long> GetCountAsync(string filterText = null, string name = null, CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, name);
            return await query.As<IMongoQueryable<Author>>().LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<Author>> GetListAsync(string filterText = null, string name = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, name);
            return await query.As<IMongoQueryable<Author>>()
                .OrderByDescending(x => x.CreationTime)
                .PageBy<Author, IMongoQueryable<Author>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Author> ApplyFilter(IQueryable<Author> query, string filterText, string name = null)
        {
            var dbContext = GetDbContextAsync();
            var getDashboards = query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Name.ToLower().Contains(filterText.ToLower()));
            return getDashboards;
        }
    }
}
