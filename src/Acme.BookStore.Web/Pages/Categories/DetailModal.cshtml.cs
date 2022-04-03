using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Categories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Acme.BookStore.Web.Pages.Categories
{
    public class DetailModalModel : BookStorePageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public CategoryDto Category { get; set; }

        public List<SelectListItem> CategoryIdFilterItems { get; set; }

        public readonly ICategoryAppService _categoryAppService;

        public DetailModalModel(ICategoryAppService categoryAppService)
        {
            _categoryAppService = categoryAppService;
        }
        public async Task OnGetAsync()
        {
            Category = await _categoryAppService.GetAsync(Id);
            
            
        }
        
    }
}

