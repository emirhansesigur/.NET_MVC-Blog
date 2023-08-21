using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogNET.Models
{
    public partial class Blog
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Baslik alanı zorunludur.")]
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public bool IsPublish { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;
        public Author Author { get; set; }
        public int AuthorId { get; set; }
        //public Category Category { get; set; } // blog eklemede hata verdigi icin sildim.

        [Display(Name = "Yuklemek İstediginiz Fotografi Seciniz")]
        [NotMapped]
        public IFormFile CoverFoto { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }


        //public int Id { get; set; }
        //public string Title { get; set; }
        //public string Subtitle { get; set; }
        //public string Content { get; set; }
        //public string ImagePath { get; set; }
        //public bool IsPublish { get; set; }
        //public int AuthorId { get; set; }
        //public int CategoryId { get; set; }
        //public DateTime CreateTime { get; set; }

        //public virtual Author Author { get; set; }
        //public string CategoryName { get; set; }
    }
}
