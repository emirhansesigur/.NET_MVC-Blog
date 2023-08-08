using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminBlog.Models{
    public class Category{
        [Key]
        public int Id { get; set; }
        //[Required(ErrorMessage = "Lütfen Bir Kategori İsmi Giriniz...")]
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }

    }
}
