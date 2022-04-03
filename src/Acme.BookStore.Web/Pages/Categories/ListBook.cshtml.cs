using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Acme.BookStore.Web.Pages.Categories
{
    public class ListBookModel : PageModel
    {
        private readonly IBookAppService _bookAppService;
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public List<BookDto> Books { get; set; }

        public ListBookModel(IBookAppService bookAppService)
        {
            _bookAppService = bookAppService;
        }
        public async Task OnGet()
        {
            Books = await _bookAppService.GetListByIdCategory(Id);
        }
    }
}
