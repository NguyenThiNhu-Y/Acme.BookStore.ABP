using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Categories;
using Acme.BookStore.Shared;
using Braintree;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Acme.BookStore.Web.Pages.Categories
{
    public class CreateModalModel : BookStorePageModel
    {
        [BindProperty]
        public CreateUpdatecategoryDto Category { get; set; }
        private readonly ICategoryAppService _categoryAppService;
        private readonly IWebHostEnvironment _hostEnvironment;
        
        [SelectItems(nameof(CategoryIdFilterItems))]
        public Guid? CategoryIdFilter { get; set; }

        public List<SelectListItem> CategoryIdFilterItems { get; set; }

        public CreateModalModel(ICategoryAppService categoryAppService, IWebHostEnvironment hostEnvironment)
        {
            _categoryAppService = categoryAppService;
            _hostEnvironment = hostEnvironment;
        }
        public async Task OnGetAsync()
        {
            Category = new CreateUpdatecategoryDto();
            CategoryIdFilterItems = new List<SelectListItem>
            {
                new SelectListItem("Choose","")
            };
            var getlist = await _categoryAppService.GetListCategoryLookupAsync();
            foreach(var item in getlist)
            {
                CategoryIdFilterItems.AddLast(new SelectListItem(item.DisplayName, item.Id.ToString()));
            }
            
            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            
            if(file != null)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                var wwwRootPath = _hostEnvironment.WebRootPath;
                var filename = "Category" + DateTime.Now.ToString("yymmssfff") + extension;
                var image = DefaultUploadImage.UploadImageCategory + filename;
                var path = Path.Combine(wwwRootPath + image);
                //if (extension == Extension.Jpg || extension == Extension.Png || extension == Extension.Svg)
                //{

                //}
                //else
                //{
                //    throw new UserFriendlyException(L["ImageIsNotCorrectFormatJpgPngSgv"]);
                //}
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                Category.Image = filename;

            }
            await _categoryAppService.CreateAsync(Category);
            return NoContent();
        }

    }
}
