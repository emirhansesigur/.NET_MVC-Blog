using AdminBlog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdminBlog.Controllers
{
    public class AuthorController : Controller
    {
        private readonly ILogger<AuthorController> _logger;
        private readonly BlogContext _context;

        public AuthorController(ILogger<AuthorController> logger, BlogContext context)
        {
            _logger = logger;
            _context = context;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddAuthor(Author author)
        {

            if (HttpContext.Session.GetString("superAdmin") != "superAdmin")
            {
                //var list = _context.Blog.ToList();    
                return RedirectToAction("Forbidden", "Error");
            }

            // SHA-256 hash hesaplama
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

                var sql = "INSERT INTO Author (Name, Surname, Email, Password) VALUES (@Name, @Surname, @Email, @Password)";

                var parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Name", author.Name));
                parameters.Add(new SqlParameter("@Surname", author.Surname));
                parameters.Add(new SqlParameter("@Email", author.Email));
                parameters.Add(new SqlParameter("@Password", hashedPassword)); // Hashlenmiş parolayı kullan

                await _context.Database.ExecuteSqlRawAsync(sql, parameters);

                return Json(true);
            }

        }

        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AuthorDetails(int Id) // ?
        {
            if (HttpContext.Session.GetString("superAdmin") != "superAdmin")
            {
                //var list = _context.Blog.ToList();    
                return RedirectToAction("Forbidden", "Error");
            }

            var author = await _context.Author.FindAsync(Id);
            return Json(author);
        }

        //public async Task<IActionResult> AuthorUpdate(int Id) // update sayfası nasıl açılıyor incele.
        //{



        //    return RedirectToAction(nameof(Author));
        //}

        //public async Task<IActionResult> AuthorUpdate(int id) // EN SON BUNDA KALDIK.
        //{


        //    var sql = "UPDATE Author SET Name = @Name, Surname = @Surname, Email = @Email, Password = @Password WHERE Id = @Id;";

        //    var parameters = new List<SqlParameter>();
        //    parameters.Add(new SqlParameter("@Name", author.Name));
        //    parameters.Add(new SqlParameter("@Surname", author.Surname));
        //    parameters.Add(new SqlParameter("@Email", author.Email));
        //    parameters.Add(new SqlParameter("@Password", author.Password));
        //    parameters.Add(new SqlParameter("@Id", author.Id));

        //    await _context.Database.ExecuteSqlRawAsync(sql, parameters.ToArray());

        //    return RedirectToAction(nameof(Author));
        //}

        //[Authorize(Roles = "SuperAdmin")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("superAdmin") != "superAdmin")
            {
                //var list = _context.Blog.ToList();    
                return RedirectToAction("Forbidden", "Error");
            }

            List<Author> list = _context.Author.OrderBy(a => a.Id).ToList();
            return View(list);
        }

        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateAuthor(Author author) // burada kaldık
        {
            if (HttpContext.Session.GetString("superAdmin") != "superAdmin")
            {
                //var list = _context.Blog.ToList();    
                return RedirectToAction("Forbidden", "Error");
            }

            var sqll = "UPDATE Author SET Name = @Name, Surname = @Surname, Email = @Email WHERE Id = @Id"; //, Password = @

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Name", author.Name));
            parameters.Add(new SqlParameter("@Surname", author.Surname));
            parameters.Add(new SqlParameter("@Email", author.Email));
            //parameters.Add(new SqlParameter("@Password", author.Password));
            parameters.Add(new SqlParameter("@Id", author.Id));
            await _context.Database.ExecuteSqlRawAsync(sqll, parameters);

            return RedirectToAction(nameof(Index));
        }

        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteAuthor(int? Id)
        {
            if (HttpContext.Session.GetString("superAdmin") != "superAdmin")
            {
                //var list = _context.Blog.ToList();    
                return RedirectToAction("Forbidden", "Error");
            }

            var sql = "DELETE FROM Author WHERE Id = @Id";
            await _context.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@Id", Id));

            return RedirectToAction(nameof(Index));
        }

    }
}
