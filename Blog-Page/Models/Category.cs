using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogNET.Models
{
    public partial class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
