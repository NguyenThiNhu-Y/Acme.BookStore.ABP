using Acme.BookStore.Shared;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Authors
{
    public interface IAuthorAppService : IApplicationService
    {
        Task<PagedResultDto<AuthorDto>> GetListAsync(GetAuthorInput input);
        Task<AuthorDto> CreateAsync(CreateUpdateAuthorDto input);

        Task<AuthorDto> ChangeNameAsync([NotNull] AuthorDto author, [NotNull] string newName);

        Task<AuthorDto> GetAsync(Guid id);

        Task<AuthorDto> UpdateAsync(Guid ID, CreateUpdateAuthorDto input);

        Task DeleteAsync(Guid Id);

        Task ChangeStatus(Guid Id);

        Task<List<LookupDto<Guid?>>> GetListAuthorLookupAsync();

    }
}
