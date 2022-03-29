using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Acme.BookStore.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Acme.BookStore.Web.Pages.Books
{
    public class EditModalModel : BookStorePageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

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
        public EditModalModel(IBookAppService bookAppService, ICategoryAppService categoryAppService, IAuthorAppService authorAppService)
        {
            _bookAppService = bookAppService;
            _categoryAppService = categoryAppService;
            _authorAppService = authorAppService;
        }
        public async Task OnGetAsync()
        {
            Book = new CreateUpdateBookDto();
            var bookDto = await _bookAppService.GetAsync(Id);
            Book = ObjectMapper.Map<BookDto, CreateUpdateBookDto>(bookDto);


            CategoryIdFilterItems = new List<SelectListItem>
            {
                new SelectListItem("Choose","")
            };
            var getlistCate = await _categoryAppService.GetListCategoryLookupAsync();
            foreach (var item in getlistCate)
            {
                CategoryIdFilterItems.AddLast(new SelectListItem(item.DisplayName, item.Id.ToString()));
            }

            AuthorIdFilterItems = new List<SelectListItem>
            {
                new SelectListItem("Chose","")
            };

            GetAuthorInput inputAuthor = new GetAuthorInput
            {
                FilterText = "",
                Name = ""
            };
            var getListAuthor = await _authorAppService.GetListAuthorLookupAsync();
            foreach(var item in getListAuthor)
            {
                AuthorIdFilterItems.AddLast(new SelectListItem(item.DisplayName, item.Id.Value.ToString()));
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _bookAppService.UpdateAsync(Id,Book);
            return NoContent();
        }
    }
}
