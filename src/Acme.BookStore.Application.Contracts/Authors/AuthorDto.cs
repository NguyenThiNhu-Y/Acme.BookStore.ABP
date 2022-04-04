using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Authors
{
    public class AuthorDto: AuditedEntityDto<Guid>
    {
        public int STT { get; set; }
        public string Name { get; set; }
        public DateTime DoB { get; set; }
        public string ShortBio { get; set; }

        public Status Status { get; set; }
        public string Image { get; set; }
    }
}
