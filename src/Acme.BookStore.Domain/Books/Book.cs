using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Books
{
    public class Book : AuditedAggregateRoot<Guid> //kế thừa các thuộc tính CreationTime, CreatorId, LastModificationTime, LastModifierId 
    {
        public string Name { get; set; }
        public Guid Type { get; set; }
        public DateTime PublishDate { get; set; }
        public float Price { get; set; }
        public Guid IdAuthor { get; set; }
        public string Image { get; set; }
        public string Describe { get; set; }
    }
}
