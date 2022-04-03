using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Acme.BookStore.Web.Pages.Categories
{
    public class EditModalModel : BookStorePageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public CreateUpdatecategoryDto Category { get; set; }

        public List<SelectListItem> CategoryIdFilterItems { get; set; }

        public readonly ICategoryAppService _categoryAppService;

        public EditModalModel(ICategoryAppService categoryAppService)
        {
            _categoryAppService = categoryAppService;
        }
        public async Task OnGetAsync()
        {
            var categoryDto = await _categoryAppService.GetAsync(Id);
            Category = ObjectMapper.Map<CategoryDto, CreateUpdatecategoryDto>(categoryDto);
            CategoryIdFilterItems = new List<SelectListItem>
            {
                new SelectListItem("Choose","")
            };
            var getList = await _categoryAppService.GetListCategoryEditLookupAsync(Id);
            foreach(var item in getList)
            {
                CategoryIdFilterItems.Add(new SelectListItem(item.DisplayName, item.Id.ToString()));
            }

        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _categoryAppService.UpdateAsync(Id, Category);
            return NoContent();
        }
    }
}
