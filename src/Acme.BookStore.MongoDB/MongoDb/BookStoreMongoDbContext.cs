using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Acme.BookStore.Categories;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Acme.BookStore.MongoDB
{

    [ConnectionStringName("Default")]
    public class BookStoreMongoDbContext : AbpMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            //builder.Entity<YourEntity>(b =>
            //{
            //    //...
            //});
        }

        public IMongoCollection<Book> Books => Collection<Book>();
        public IMongoCollection<Category> Categories => Collection<Category>();

        public IMongoCollection<Author> Authors => Collection<Author>();
    }
}