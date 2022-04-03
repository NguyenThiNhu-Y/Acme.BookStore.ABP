using Acme.BookStore.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Books
{
    public interface IBookAppService : IApplicationService
    {
        //Task<PagedResultDto<BookView>> GetListAsync(GetBookInput input);
        Task<PagedResultDto<BookDto>> GetListAsync(GetBookInput input);

        Task<BookDto> GetAsync(Guid id);

        Task<BookDto> CreateAsync(CreateUpdateBookDto input);
        Task<BookDto> UpdateAsync(Guid id, CreateUpdateBookDto input);
        Task DeleteAsync(Guid id);
        Task<List<LookupDto<Guid?>>> GetListCategoryLookupAsync();

        Task<List<BookDto>> GetListByIdAuthor(Guid idAuthor);
        Task<List<BookDto>> GetListByIdCategory(Guid idCategory);
    }
}
