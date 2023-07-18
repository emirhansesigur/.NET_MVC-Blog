using System;
using System.ComponentModel.DataAnnotations;

namespace AdminBlog.Models{
    public class Author{
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [Required(ErrorMessage = "Lütfen Bir Email Değeri Giriniz...")]
        [EmailAddress(ErrorMessage = "Lütfen Doğru Formda Email Giriniz...")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lütfen Bir Şifre Giriniz")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "Şifre 6-16 karakter uzunluğunda olmalıdır.")]
        public string Password { get; set; }
    }
}
