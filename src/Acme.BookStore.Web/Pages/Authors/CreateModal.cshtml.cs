using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Authors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Acme.BookStore.Web.Pages.Authors
{
    public class CreateModalModel : BookStorePageModel
    {
        [BindProperty]
        public CreateUpdateAuthorDto Author { get; set; }
        private readonly IAuthorAppService _authorAppService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CreateModalModel(IAuthorAppService authorAppService, IWebHostEnvironment hostEnvironment)
        {
            _authorAppService = authorAppService;
            _hostEnvironment = hostEnvironment;
        }
        public void OnGet()
        {
            Author = new CreateUpdateAuthorDto();
        }

        public async Task<IActionResult> OnPost()
        {
            //if (file != null)
            //{
            //    var extension = Path.GetExtension(file.FileName).ToLower();
            //    var wwwRootPath = _hostEnvironment.WebRootPath;
            //    var filename = "Author" + DateTime.Now.ToString("yymmssfff") + extension;
            //    var image = DefaultUploadImage.UploadImageAuthor + filename;
            //    var path = Path.Combine(wwwRootPath + image);

            //    using (var stream = new FileStream(path, FileMode.Create))
            //    {
            //        file.CopyTo(stream);
            //    }
            //    Author.Image = filename;

            //}
            if (ModelState.IsValid)
            {
                await _authorAppService.CreateAsync(Author);
                return RedirectToAction("Index", "Authors");

            }

            return NoContent();
        }
    }
}
