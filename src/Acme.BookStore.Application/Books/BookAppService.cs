using Acme.BookStore.Authors;
using Acme.BookStore.Categories;
using Acme.BookStore.Permissions;
using Acme.BookStore.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Books
{
    public class BookAppService : ApplicationService ,IBookAppService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryAppService _categoryAppService;
        private readonly IAuthorRepository _authorRepository;

        //public string GetPolicyName { get; }
        //public string GetListPolicyName { get; }
        //public string CreatePolicyName { get; }
        //public string UpdatePolicyName { get; }
        //public string DeletePolicyName { get; }

        public BookAppService(IBookRepository bookRepository, ICategoryRepository categoryRepository, ICategoryAppService categoryAppService, IAuthorRepository authorRepository) //: base(categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _categoryAppService = categoryAppService;
            _authorRepository = authorRepository;

            //GetPolicyName = BookStorePermissions.Books.Default;
            //GetListPolicyName = BookStorePermissions.Books.Default;
            //CreatePolicyName = BookStorePermissions.Books.Create;
            //UpdatePolicyName = BookStorePermissions.Books.Edit;
            //DeletePolicyName = BookStorePermissions.Books.Delete;
        }

        public async Task<BookDto> CreateAsync(CreateUpdateBookDto input)
        {
            var book = ObjectMapper.Map<CreateUpdateBookDto, Book>(input);
            await _bookRepository.InsertAsync(book);
            await _categoryAppService.UpdateCountBook(book.Type, true);
            return ObjectMapper.Map<Book, BookDto>(book);

        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await _bookRepository.FindAsync(id);
            await _categoryAppService.UpdateCountBook(book.Type, false);
            await _bookRepository.DeleteAsync(book);
        }

        public async Task<BookDto> GetAsync(Guid id)
        {
            var book = await _bookRepository.FindAsync(id);
            return ObjectMapper.Map<Book, BookDto>(book);
        }

        //public async Task<PagedResultDto<BookView>> GetListAsync(GetBookInput input)
        //{
        //    var count = await _bookRepository.GetCountAsync(input.FilterText, input.Name);
        //    var items = await _bookRepository.GetListAsync(input.FilterText, input.Name, input.Sorting, input.MaxResultCount, input.SkipCount);
        //    var result = new List<BookView>();
        //    foreach(var i in items)
        //    {
        //        result.Add(new BookView()
        //        {
        //            //Category = (await _categoryRepository.GetAsync(i.Type))?.Name,
        //            Category = "",
        //            Name = i.Name,
        //            PublishDate = i.PublishDate,
        //            Price = i.Price
        //        });
        //    }
        //    return new PagedResultDto<BookView> {
        //        TotalCount = count,
        //        Items = result
        //    };
        //}

        public async Task<PagedResultDto<BookDto>> GetListAsync(GetBookInput input)
        {
            var count = await _bookRepository.GetCountAsync(input.FilterText, input.Name);
            var items = await _bookRepository.GetListAsync(input.FilterText, input.Name, input.Sorting, input.MaxResultCount, input.SkipCount);
            var result = new List<BookDto>();
            foreach (var i in items)
            {
                var category = (await _categoryRepository.FindAsync(i.Type));
                var author = await _authorRepository.FindAsync(i.IdAuthor);
                string ctgParent = "";
                if (category != null)
                {
                    ctgParent = category.Name;
                }
                string authorName = "";
                if (author != null)
                {
                    authorName = author.Name;
                }
                result.Add(new BookDto()
                {

                    CategoryParent = ctgParent,
                    Name = i.Name,
                    PublishDate = i.PublishDate,
                    Price = i.Price,
                    Type = i.Type,
                    Id = i.Id,
                    Author = authorName,
                    IdAuthor = i.IdAuthor
                }) ;
            }
            return new PagedResultDto<BookDto>
            {
                TotalCount = count,
                Items = (result)
            };
        }

        public async Task<List<BookDto>> GetListByIdAuthor(Guid idAuthor)
        {

            var listAllBooks = await  _bookRepository.GetListAsync();
            List<BookDto> result = new List<BookDto>();
            foreach(var item in listAllBooks)
            {
                var category = (await _categoryRepository.FindAsync(item.Type));
                string ctgParent = "";
                if (category != null)
                {
                    ctgParent = category.Name;
                }
                
                if (item.IdAuthor == idAuthor)
                {
                    result.Add(new BookDto()
                    {

                        CategoryParent = ctgParent,
                        Name = item.Name,
                        PublishDate = item.PublishDate,
                        Price = item.Price,
                        Type = item.Type,
                        Id = item.Id,
                        IdAuthor = item.IdAuthor
                    });
                }
            }
            return result;
        }

        public async Task<List<LookupDto<Guid?>>> GetListCategoryLookupAsync()
        {
            var queryable = await _bookRepository.GetQueryableAsync();
            var query = from category in queryable
                        select new
                        {
                            ID = category.Id,
                            Name = category.Name
                        };
            var ListCategory = query.ToList();
            
            List<LookupDto<Guid?>> list = new List<LookupDto<Guid?>>();
            foreach (var item in ListCategory)
            {
                list.Add(new LookupDto<Guid?>
                {
                    Id = item.ID,
                    DisplayName = item.Name
                });
            }
            return list;

        }

        public async Task<BookDto> UpdateAsync(Guid id, CreateUpdateBookDto input)
        {
            var book = ObjectMapper.Map<CreateUpdateBookDto, Book>(input);
            var oldBook = await _bookRepository.FindAsync(id);
            oldBook.Name = book.Name;
            oldBook.Price = book.Price;
            oldBook.Type = book.Type;
            oldBook.PublishDate = book.PublishDate;
            oldBook.IdAuthor = book.IdAuthor;
            await _bookRepository.UpdateAsync(oldBook);
            return ObjectMapper.Map<Book, BookDto>(oldBook);
        }
    }
}
