using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Authors;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Acme.BookStore.Web.Pages.Authors
{
    public class IndexModel : PageModel
    {
        private readonly IAuthorRepository _authorRepository;

        [BindProperty]
        public List<AuthorDto> authorDtos { get; set; }
        public IndexModel(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task OnGetAsync(string SearchString)
        {
            authorDtos = new List<AuthorDto>();
            var rs = await _authorRepository.GetListAsync(name: SearchString);
        }
       
    }
}
