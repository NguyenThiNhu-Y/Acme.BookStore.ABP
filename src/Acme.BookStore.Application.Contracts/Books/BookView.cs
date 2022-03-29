using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.BookStore.Books
{
    
   public class BookView : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string? Category { get; set; }
        public DateTime PublishDate { get; set; }
        public float Price { get; set; }

    }
}
