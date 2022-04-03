using Acme.BookStore.Books;
using Acme.BookStore.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;


namespace Acme.BookStore.Categories
{
    public class CategoryAppService : ApplicationService, ICategoryAppService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBookRepository _bookRepository;

        public CategoryAppService(ICategoryRepository categoryRepository, IBookRepository  bookRepository)
        {
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
        }

        public async Task<int> UpdateCountBook(Guid id, bool add)
        {
            var category = await _categoryRepository.FindAsync(id);
            if (add)
            {
                category.CountBook++;
            }
            else
            {
                category.CountBook--;
            }
            
            await _categoryRepository.UpdateAsync(category);
            return category.CountBook;
        }

        public async Task<CategoryDto> CreateAsync(CreateUpdatecategoryDto input)
        {
            input.Status = Status.Visibility;
            var categrory = ObjectMapper.Map<CreateUpdatecategoryDto, Category>(input);
            await _categoryRepository.InsertAsync(categrory);
            return ObjectMapper.Map<Category, CategoryDto>(categrory);
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            var category = await _categoryRepository.FindAsync(Id);

            var listBook = await _bookRepository.GetListAsync();
            int count = 0;
            foreach (var item in listBook)
            {
                if (item.Type == Id)
                    count++;

            }
            if (count == 0)
            {
                await _categoryRepository.DeleteAsync(category);
                return true;
            }
            return false;
        }

        public async Task<CategoryDto> GetAsync(Guid id)
        {
            var category = await _categoryRepository.FindAsync(id);
            Category categoryParent = new Category();
            if (category.IdParen != null)
            {
                categoryParent = await _categoryRepository.FindAsync(category.IdParen.Value);
            }
            
            string ctgParent = " ";
            if (categoryParent != null)
            {
                
                    ctgParent = categoryParent.Name;
                
            }
            var result = ObjectMapper.Map<Category, CategoryDto>(category);
            result.CategoryParent = ctgParent;
            return result;
        }

        

        public async Task<PagedResultDto<CategoryDto>> GetListAsync(GetCategroryInput input)
        {
            var count = await _categoryRepository.GetCountAsync(input.FilterText, input.Name);
            var items = await _categoryRepository.GetListAsync(input.FilterText, input.Name, input.Sorting, input.MaxResultCount,input.SkipCount);
            var listBook = await _bookRepository.GetListAsync();

            List<CategoryDto> result = new List<CategoryDto>();
            foreach (var i in items)
            {
                string ctgParent = " ";
                if (i.IdParen != null)
                {
                    var category = await _categoryRepository.FindAsync(Guid.Parse(i.IdParen.ToString()));
                    
                    if (category != null)
                    {
                        ctgParent = category.Name;
                    }
                }
                int countBooks = 0;
                foreach (var item in listBook)
                {
                    if (item.Type == i.Id)
                        countBooks++;

                }
                
                result.Add(new CategoryDto()
                {
                    Id = i.Id,
                    Name = i.Name,
                    CategoryParent = ctgParent,
                    Image = i.Image,
                    Describe = i.Describe,
                    CountBook = countBooks,
                    Status = i.Status,
                    IdParen = i.IdParen,
                    
                });
            }
            return new PagedResultDto<CategoryDto>
            {
                TotalCount = count,
                //Items = ObjectMapper.Map<List<Category>, List<CategoryDto>>(items)
                Items = result
            };
        }

        //public async Task<PagedResultDto<LookupDto<Guid?>>> GetListCategoryLookupAsync(LookupRequestDto input)
        //{
        //    var queryable = await _categoryRepository.GetQueryableAsync();
        //    var query = from category in queryable
        //                select new
        //                {
        //                    ID = category.Id,
        //                    Name = category.Name
        //                };
        //    var ListCategory = query.ToList();
        //    var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<CategoryDto>();
        //    var totalCount = query.Count();
        //    return new PagedResultDto<LookupDto<Guid?>>
        //    {
        //        TotalCount = totalCount,
        //        Items = ObjectMapper.Map<List<CategoryDto>, List<LookupDto<Guid?>>>(lookupData)
        //    };

        //}

        public async Task<List<LookupDto<Guid?>>> GetListCategoryLookupAsync()
        {
            var queryable = await _categoryRepository.GetQueryableAsync();
            var query = from category in queryable
                        select new
                        {
                            ID = category.Id,
                            Name = category.Name
                        };
            var ListCategory = query.ToList();
            //var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<CategoryDto>();
            //var totalCount = query.Count();
            //return new PagedResultDto<LookupDto<Guid?>>
            //{
            //    TotalCount = totalCount,
            //    Items = ObjectMapper.Map<List<CategoryDto>, List<LookupDto<Guid?>>>(lookupData)
            //};
            List<LookupDto<Guid?>> list = new List<LookupDto<Guid?>>();
            foreach (var item in ListCategory)
            {
                
                {
                    list.Add(new LookupDto<Guid?>
                    {
                        Id = item.ID,
                        DisplayName = item.Name
                    });
                }
                
            }
            return list;

        }

        public async Task<CategoryDto> UpdateAsync(Guid id, CreateUpdatecategoryDto input)
        {
            var oldCategory = await _categoryRepository.FindAsync(id);
            oldCategory.Name = input.Name;
            oldCategory.Image = input.Image;
            oldCategory.Describe = input.Describe;
            oldCategory.IdParen = input.IdParen;
            oldCategory.Status = input.Status;
            await _categoryRepository.UpdateAsync(oldCategory);
            return ObjectMapper.Map<Category, CategoryDto>(oldCategory);
        }

        public async Task ChangStatus(Guid id)
        {
            var category = await _categoryRepository.GetAsync(id);
            if(category.Status == Status.Hide)
            {
                category.Status = Status.Visibility;
            }
            else
            {
                category.Status = Status.Hide;
            }
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task<List<LookupDto<Guid?>>> GetListCategoryEditLookupAsync(Guid Id)
        {
            List<LookupDto<Guid?>> list = await GetListCategoryLookupAsync();
            List<LookupDto<Guid?>> listResult = new List<LookupDto<Guid?>>();
            foreach (var item in list)
            {
                if(item.Id != Id)
                {
                    listResult.Add(new LookupDto<Guid?>
                    {
                        Id = item.Id,
                        DisplayName = item.DisplayName
                    });
                }

            }
            return listResult;

        }
    }
}
