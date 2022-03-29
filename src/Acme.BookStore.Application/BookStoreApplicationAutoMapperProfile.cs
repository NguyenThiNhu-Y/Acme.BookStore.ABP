using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Acme.BookStore.Categories;
using AutoMapper;

namespace Acme.BookStore
{

    public class BookStoreApplicationAutoMapperProfile : Profile
    {
        public BookStoreApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<Book, BookDto>(); //ánh xa Book với BookDto
            CreateMap<CreateUpdateBookDto, Book>();
            CreateMap<BookDto, CreateUpdateBookDto>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CreateUpdatecategoryDto, Category>();

            CreateMap<CreateUpdateAuthorDto, Author>();
            CreateMap<Author, AuthorDto>();
            CreateMap<AuthorDto, Author>();
            CreateMap<AuthorDto, CreateUpdateAuthorDto>();
        }
    }
}