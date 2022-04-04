using Acme.BookStore.Books;
using Acme.BookStore.Shared;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.ObjectMapping;

namespace Acme.BookStore.Authors
{
    public class AuthorAppService : ApplicationService, IAuthorAppService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IWebHostEnvironment _hostEnvironment;

        private static int SkipCount=0;
        private static int MaxResultCount=0;

        public AuthorAppService(IAuthorRepository authorRepository, IBookRepository bookRepository, IWebHostEnvironment hostEnvironment)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<AuthorDto> ChangeNameAsync([NotNull] AuthorDto author, [NotNull] string newName)
        {
            Check.NotNull(author, nameof(author));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var authorEnity = ObjectMapper.Map<AuthorDto, Author>(author);
            var existingAuthor =  await _authorRepository.FindByNameAsync(newName);
            if (existingAuthor != null && existingAuthor.Id != author.Id)
            {
                throw new AuthorAlreadyExistsException(newName);
            }

            authorEnity.ChangeName(newName);
            return ObjectMapper.Map<Author, AuthorDto>(authorEnity);
        }

        public async Task ChangeStatus(Guid Id)
        {
            var author = await _authorRepository.FindAsync(Id);
            if(author.Status == Status.hide)
            {
                author.Status = Status.visible;
            }
            else
            {
                author.Status = Status.hide;
            }
            await _authorRepository.UpdateAsync(author);
        }

        public async Task<AuthorDto> CreateAsync(CreateUpdateAuthorDto input)
        {
            //Check.NotNullOrWhiteSpace(input.Name, nameof(input.Name));
            //var existingAuthor = await _authorRepository.FindByNameAsync(input.Name);
            //if(existingAuthor != null)
            //{
            //    throw new AuthorAlreadyExistsException(input.Name);
            //}
            
            var author = ObjectMapper.Map<CreateUpdateAuthorDto, Author>(input);
            author.Status = Status.visible;
            await _authorRepository.InsertAsync(author);
            return ObjectMapper.Map<Author, AuthorDto>(author);

            
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            var author = await _authorRepository.FindAsync(Id);
            var listBook = await _bookRepository.GetListAsync();
            int count = 0;
            foreach(var item in listBook)
            {
                if (item.IdAuthor == Id)
                    count++;

            }
            if(count == 0)
            {
                await _authorRepository.DeleteAsync(author);
                return true;
            }
            return false;
            
        }

        public async Task<AuthorDto> GetAsync(Guid id)
        {
            var author = await _authorRepository.FindAsync(id);
            return ObjectMapper.Map<Author, AuthorDto>(author);
        }

        public async Task<PagedResultDto<AuthorDto>> GetListAsync(GetAuthorInput input)
        {
            List<Author> items = new List<Author>();
            if (SkipCount == 0 && MaxResultCount == 0)
            {
                SkipCount = input.SkipCount;
                MaxResultCount = input.MaxResultCount;
            }

            var count = await _authorRepository.GetCountAsync(input.FilterText, input.Name);
            //if (input.FilterText != null)
            //{
            //    items = await _authorRepository.GetListAsync(input.FilterText, input.Name, input.Sorting, MaxResultCount, SkipCount);
            //}
            //else
            {
                items = await _authorRepository.GetListAsync(input.FilterText, input.Name, input.Sorting, input.MaxResultCount, input.SkipCount);
            }
            var rs = ObjectMapper.Map<List<Author>, List<AuthorDto>>(items);
            var index = input.SkipCount+1;
            List<AuthorDto> result = new List<AuthorDto>();
            foreach(var item in rs)
            {
                item.STT = index++;
            }
            //if (input.FilterText != null)
            //{
            //    for(int i = SkipCount + 1; i <= SkipCount + MaxResultCount; i++)
            //    {
            //        result.Add(rs[i]);
            //    }
            //    return new PagedResultDto<AuthorDto>
            //    {
            //        TotalCount = count,
            //        Items = result
            //    };
            //}
            SkipCount = input.SkipCount;
            MaxResultCount = input.MaxResultCount;
            return new PagedResultDto<AuthorDto>
            {
                TotalCount = count,
                Items = rs
            };

        }

        public async Task<List<LookupDto<Guid?>>> GetListAuthorLookupAsync()
        {
            var queryable = await _authorRepository.GetQueryableAsync();
            var query = from author in queryable
                        select new
                        {
                            Id = author.Id,
                            Name = author.Name
                        };
            var listAuthor = query.ToList();
            List<LookupDto<Guid?>> list = new List<LookupDto<Guid?>>();
            foreach (var item in listAuthor)
            {
                list.Add(new LookupDto<Guid?>
                {
                    Id = item.Id,
                    DisplayName = item.Name
                });
            }
            return list;
        }

        public async Task<AuthorDto> UpdateAsync(Guid ID, CreateUpdateAuthorDto input)
        {
            var author = await _authorRepository.FindAsync(ID);
            var authorDto = ObjectMapper.Map<Author, AuthorDto>(author);
            //await ChangeNameAsync(authorDto, input.Name);
            author.Name = input.Name;
            author.DoB = input.DoB;
            author.ShortBio = input.ShortBio;
            await _authorRepository.UpdateAsync(author);
            return ObjectMapper.Map<Author, AuthorDto>(author);
        }

        public string UploadImage(IFormFile file)
        {
            string filePath = "";
            if (file!= null)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                var wwwRootPath = _hostEnvironment.WebRootPath;
                var filename = "Author" + DateTime.Now.ToString("yymmssfff") + extension;
                var image = DefaultUploadImage.UploadImageAuthor + filename;
                var path = Path.Combine(wwwRootPath + image);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                filePath = "https://localhost:44338/ImageAuthor/" + filename;

            }
            return filePath;
        }
    }
}
