using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Authors;
using Acme.BookStore.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Acme.BookStore.Web.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryAppService _categoryAppService;
        private readonly IAuthorAppService _authorAppService;


        [SelectItems(nameof(CategoryIdFilterItems))]
        public Guid? CategoryId { get; set; }
        public List<SelectListItem> CategoryIdFilterItems { get; set; }

        [SelectItems(nameof(AuthorIdFilterItems))]
        public Guid? AuthorId { get; set; }
        public List<SelectListItem> AuthorIdFilterItems { get; set; }

        public List<Shared.LookupDto<Guid?>> getlistCategory;
        public List<Shared.LookupDto<Guid?>> getListAuthor;

        public IndexModel(ICategoryAppService categoryAppService, IAuthorAppService authorAppService)
        {
            _categoryAppService = categoryAppService;
            _authorAppService = authorAppService;
        }
        public async Task OnGet()
        {
            getlistCategory = new List<Shared.LookupDto<Guid?>>();
            getListAuthor = new List<Shared.LookupDto<Guid?>>();
            getlistCategory = await _categoryAppService.GetListCategoryLookupAsync();
            CategoryIdFilterItems = new List<SelectListItem>{
                new SelectListItem("Choose","")
            };
            foreach (var item in getlistCategory)
            {
                CategoryIdFilterItems.AddLast(new SelectListItem(item.DisplayName, item.Id.ToString()));
            }

            AuthorIdFilterItems = new List<SelectListItem>
            { new SelectListItem("Choose","")
            };


            GetAuthorInput inputAuthor = new GetAuthorInput
            {
                FilterText = "",
                Name = ""
            };
            getListAuthor = await _authorAppService.GetListAuthorLookupAsync();
            foreach (var item in getListAuthor)
            {
                AuthorIdFilterItems.AddLast(new SelectListItem(item.DisplayName, item.Id.Value.ToString()));
            }
        }
    }
}
