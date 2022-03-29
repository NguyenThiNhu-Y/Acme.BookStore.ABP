using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Shared
{
    public class LookupRequestDto : PagedResultRequestDto
    {
        public string Filter { get; set; }

        public LookupRequestDto()
        {
            MaxResultCount = MaxMaxResultCount;
        }
    }
}
