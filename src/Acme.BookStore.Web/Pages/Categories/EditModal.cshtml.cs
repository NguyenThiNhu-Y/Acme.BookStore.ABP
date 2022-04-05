using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Categories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private readonly IWebHostEnvironment _hostEnvironment;

        public EditModalModel(ICategoryAppService categoryAppService, IWebHostEnvironment hostEnvironment)
        {
            _categoryAppService = categoryAppService;
            _hostEnvironment = hostEnvironment;
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
        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    var wwwRootPath = _hostEnvironment.WebRootPath;
                    var filename = "Category" + DateTime.Now.ToString("yymmssfff") + extension;
                    var image = DefaultUploadImage.UploadImageCategory + filename;
                    var path = Path.Combine(wwwRootPath + image);
                    
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    Category.Image = filename;

                }
                await _categoryAppService.UpdateAsync(Id, Category);
                return RedirectToAction("Index", "Categories");
            }
            return NoContent();
        }
    }
}
