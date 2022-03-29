using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.BookStore.Categories
{
    public class CategoryNavigation
    {
        public Category Category { get; set; }
        public Category CategoryParent { get; set; }
    }
}
