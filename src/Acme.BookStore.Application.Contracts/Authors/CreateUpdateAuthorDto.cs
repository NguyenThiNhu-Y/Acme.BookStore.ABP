using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Acme.BookStore.Authors
{
    public class CreateUpdateAuthorDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DoB { get; set; }

        public string ShortBio { get; set; }
    }
}
