using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Books
{
    public class BookDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public Guid Type { get; set; }
        public DateTime PublishDate { get; set; }
        public float Price { get; set; }
        public string? CategoryParent { get; set; }
        public Guid IdAuthor { get; set; }
        public string Author { get; set; }
    }
}
