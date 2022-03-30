using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Acme.BookStore.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Acme.BookStore.Web.Pages.Books
{
    public class CreateModalModel : BookStorePageModel
    {
        [BindProperty]
        public CreateUpdateBookDto Book { get; set; }

        private readonly IBookAppService _bookAppService;
        private readonly ICategoryAppService _categoryAppService;
        private readonly IAuthorAppService _authorAppService;


        [SelectItems(nameof(CategoryIdFilterItems))]
        public Guid? CategoryId { get; set; }
        public List<SelectListItem> CategoryIdFilterItems { get; set; }

        [SelectItems(nameof(AuthorIdFilterItems))]
        public Guid? AuthorId { get; set; }
        public List<SelectListItem> AuthorIdFilterItems { get; set; }

        public CreateModalModel(IBookAppService bookAppService, ICategoryAppService categoryAppService, IAuthorAppService authorAppService)
        {
            _bookAppService = bookAppService;
            _categoryAppService = categoryAppService;
            _authorAppService = authorAppService;
        }

        public async void OnGet()
        {
            Book = new CreateUpdateBookDto();
            CategoryIdFilterItems = new List<SelectListItem>();
            
            var getlist = await _categoryAppService.GetListCategoryLookupAsync();
            foreach (var item in getlist)
            {
                CategoryIdFilterItems.AddLast(new SelectListItem(item.DisplayName, item.Id.ToString()));
            }

            AuthorIdFilterItems = new List<SelectListItem>();
            

            GetAuthorInput inputAuthor = new GetAuthorInput
            {
                FilterText = "",
                Name = ""
            };
            var getListAuthor = await _authorAppService.GetListAuthorLookupAsync();
            foreach (var item in getListAuthor)
            {
                AuthorIdFilterItems.AddLast(new SelectListItem(item.DisplayName, item.Id.Value.ToString()));
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _bookAppService.CreateAsync(Book);
            
            
            return NoContent();
        }
    }
}
