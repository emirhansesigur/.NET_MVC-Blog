using System;
using System.Collections.Generic;

namespace BlogNET.Models
{
    public partial class Author
    {
        public Author()
        {
            Blog = new HashSet<Blog>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Blog> Blog { get; set; }
    }
}
