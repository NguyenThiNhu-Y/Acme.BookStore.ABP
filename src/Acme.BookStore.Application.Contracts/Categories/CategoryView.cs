using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.BookStore.Categories
{
    public class CategoryView
    {
        public string Name { get; set; }
        public string CategoryParent { get; set; }
        public string Image { get; set; }
        public string Describe { get; set; }
        public string CountBook { get; set; }
        public Status Status { get; set; }
    }
}
