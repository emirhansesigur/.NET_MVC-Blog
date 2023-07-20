using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminBlog.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Baslik alanı zorunludur.")]
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public bool IsPublish { get; set; }

        public DateTime CreateTime { get; set; }=DateTime.Now;
        public Author Author { get; set; }
        public int AuthorId { get; set; }
        public Category Category { get; set; } // blog eklemede hata verdigi icin sildim.

        [Display(Name ="Yuklemek İstediginiz Fotografi Seciniz")]
        [NotMapped]
        public IFormFile CoverFoto { get; set; }
        public int CategoryId { get; set; }


    }
}
