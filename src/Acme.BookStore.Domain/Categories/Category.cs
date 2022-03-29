using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Categories
{
    public class Category : AuditedAggregateRoot<Guid> //kế thừa để tạo entity
    {
        public string Name { get; set; }
        public Guid? IdParen { get; set; }
        public string Image { get; set; }
        public string Describe { get; set; }
        public int CountBook { get; set; }
        public Status Status { get; set; }
    }
}
