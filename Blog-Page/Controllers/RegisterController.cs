using BlogNET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlogNET.Controllers
{
    public class RegisterController : Controller
    {
        private readonly BlogDbContext _context;


        public RegisterController(BlogDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Index(Author author)
        {
            
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(author.Password); // Parolayı byte dizisine dönüştür
                byte[] hashBytes = sha256.ComputeHash(passwordBytes); // Byte dizisini SHA-256 ile hashle

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // Her byte'ı hexadecimal formata dönüştürerek string olarak birleştir
                }

                string hashedPassword = sb.ToString();

                var sql = "INSERT INTO Author (Name, Surname, Email, Password, Role) VALUES (@Name, @Surname, @Email, @Password, @Role)";

                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Name", author.Name));
                parameters.Add(new SqlParameter("@Surname", author.Surname));
                parameters.Add(new SqlParameter("@Email", author.Email));
                parameters.Add(new SqlParameter("@Password", hashedPassword)); // Hashlenmiş parolayı kullan
                parameters.Add(new SqlParameter("@Role", "Admin")); // Hashlenmiş parolayı kullan

                await _context.Database.ExecuteSqlRawAsync(sql, parameters);
                ViewBag.kayitbasarili = true;

                
                return View();
            }

        }
    }
}
