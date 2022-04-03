using Acme.BookStore.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Categories
{
    public interface ICategoryAppService : IApplicationService
    {
        Task<PagedResultDto<CategoryDto>> GetListAsync(GetCategroryInput input);
        Task<CategoryDto> CreateAsync(CreateUpdatecategoryDto input);

        Task<CategoryDto> UpdateAsync(Guid id, CreateUpdatecategoryDto input);

        Task<CategoryDto> GetAsync(Guid id);

        Task<bool> DeleteAsync(Guid Id);

        //Task<PagedResultDto<LookupDto<Guid?>>> GetListCategoryLookupAsync(LookupRequestDto input);

        Task<List<LookupDto<Guid?>>> GetListCategoryLookupAsync();
        Task<List<LookupDto<Guid?>>> GetListCategoryEditLookupAsync(Guid Id);

        Task<int> UpdateCountBook(Guid id, bool add);

        Task ChangStatus(Guid id);


    }
}
