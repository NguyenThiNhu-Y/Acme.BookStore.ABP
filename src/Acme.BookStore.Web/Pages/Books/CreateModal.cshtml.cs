using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Acme.BookStore.Categories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using DefaultUploadImage = Acme.BookStore.Books.DefaultUploadImage;

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

        private readonly IWebHostEnvironment _hostEnvironment;

        public CreateModalModel(IBookAppService bookAppService, ICategoryAppService categoryAppService, IAuthorAppService authorAppService, IWebHostEnvironment hostEnvironment)
        {
            _bookAppService = bookAppService;
            _categoryAppService = categoryAppService;
            _authorAppService = authorAppService;
            _hostEnvironment = hostEnvironment;
        }

        public async Task OnGet()
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

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if(file!= null)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                var wwwRootPath = _hostEnvironment.WebRootPath;
                var filename = "Book" + DateTime.Now.ToString("yymmssfff") + extension;
                var image = DefaultUploadImage.UploadImageBook + filename;
                var path = Path.Combine(wwwRootPath + image);
                
                
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                Book.Image = filename;
            }
            await _bookAppService.CreateAsync(Book);
            return NoContent();
        }
    }
}
