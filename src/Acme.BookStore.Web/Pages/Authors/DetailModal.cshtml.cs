using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Authors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Acme.BookStore.Web.Pages.Authors
{
    public class DetailModalModel : BookStorePageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public CreateUpdateAuthorDto Author { get; set; }
        private readonly IAuthorAppService _authorAppService;
        public DetailModalModel(IAuthorAppService authorAppService)
        {
            _authorAppService = authorAppService;
        }
        public async Task OnGetAsync()
        {
            Author = new CreateUpdateAuthorDto();
            var authorDto = await _authorAppService.GetAsync(Id);
            Author = ObjectMapper.Map<AuthorDto, CreateUpdateAuthorDto>(authorDto);

        }
        public async Task<IActionResult> OnPostAsync()
        {

            await _authorAppService.UpdateAsync(Id, Author);
            return NoContent();
        }
    }
}
