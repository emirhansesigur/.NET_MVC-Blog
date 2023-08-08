using System;
using System.Collections.Generic;

namespace BlogNET.Models
{
    public partial class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
