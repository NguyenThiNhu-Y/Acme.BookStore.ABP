using Acme.BookStore.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Acme.BookStore.Categories
{
    public class MongoCategoryRepository : MongoDbRepository<BookStoreMongoDbContext, Category, Guid>, ICategoryRepository
    {
        public MongoCategoryRepository(IMongoDbContextProvider<BookStoreMongoDbContext> dbContext) : base(dbContext)
        {

        }

        public async Task<long> GetCountAsync(string filterText = null, string name = null, CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, name);
            return await query.As<IMongoQueryable<Category>>().LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<Category>> GetListAsync(string filterText = null, string name = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, name);
            return await query.As<IMongoQueryable<Category>>()
                .OrderByDescending(x => x.CreationTime)
                .PageBy<Category, IMongoQueryable<Category>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<CategoryNavigation>> GetListNavigation(string filterText = null, string name = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetMongoQueryableAsync(cancellationToken)), filterText, name);
            var listCategory = await GetListAsync();
            var dbCOntext = await GetDbContextAsync(cancellationToken);
            List<CategoryNavigation> listCtegoryNavigation = new List<CategoryNavigation>();
            foreach (var c in listCategory)
            {
                var category = c;
                var categoryParent = dbCOntext.Categories.AsQueryable().FirstOrDefault(x => x.Id == c.IdParen);
                listCtegoryNavigation.Add(new CategoryNavigation
                {
                    Category = category,
                    CategoryParent = categoryParent
                });
            }
            return listCtegoryNavigation;
        }
        protected virtual IQueryable<Category> ApplyFilter(
            IQueryable<Category> query,
            string filterText, string name = null)
        {
            var dbContext = GetDbContextAsync();
            var getDashboards = query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Name.ToLower().Contains(filterText.ToLower()));
            return getDashboards;
        }
    }
}
