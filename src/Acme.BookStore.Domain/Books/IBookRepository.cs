using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Books
{
    public interface IBookRepository : IRepository<Book, Guid>
    {
        Task<List<Book>> GetListAsync(
           
            string filterText = null,
            string name = null,
             Guid? IdAuthor = null,
            Guid? IdCategory = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
            );

        Task<long> GetCountAsync(
            string filterText = null,
            string name = null,
            Guid? IdAuthor = null,
            Guid? IdCategory = null,
            CancellationToken cancellationToken = default);
    }
}
