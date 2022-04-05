using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Acme.BookStore.Categories
{
    public class CreateUpdatecategoryDto
    {
     
        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        public Guid? IdParen { get; set; }

        public string Image { get; set; }

        public string Describe { get; set; }

        public Status Status { get; set; }
    }
}
