using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.BookStore.Shared
{
    public class LookupDto<TKey>
    {
        public TKey Id { get; set; }
        public string DisplayName { get; set; }
    }
}
