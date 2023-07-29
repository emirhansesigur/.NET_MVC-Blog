using BlogNET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlogNET.Controllers
{
    public class LoginController : Controller
    {
        //private readonly ILogger<LoginController> _logger;
        private readonly BlogDbContext _context;


        public LoginController(/*ILogger<LoginController> logger,*/ BlogDbContext context)
        {
            //_logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        //[AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Index(Author model) // home login e gidiyor. o yüzden düzeltmeler yappp.
        {
            using (SHA256 sha256 = SHA256.Create())
            {

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                byte[] passwordBytes = Encoding.UTF8.GetBytes(model.Password); // Parolayı byte dizisine dönüştür
                byte[] hashBytes = sha256.ComputeHash(passwordBytes); // Byte dizisini SHA-256 ile hashle

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // Her byte'ı hexadecimal formata dönüştürerek string olarak birleştir
                }

                string hashedPassword = sb.ToString();

                var emailParam = new SqlParameter("@Email", model.Email);
                var passwordParam = new SqlParameter("@Password", hashedPassword);

                var query = "SELECT TOP 1 * FROM Author WHERE Email = @Email AND Password = @Password";
                //TOP 1 İstenen sonucun sadece ilk kaydı olduğu biliniyorsa, bu ifade kullanılarak sorgunun performansı artırılabilir.

                var author = await _context.Author.FromSqlRaw(query, emailParam, passwordParam).FirstOrDefaultAsync();

                if (author == null)
                {
                    // BURADA KALDIK

                    return RedirectToAction(nameof(Index));
                }

                HttpContext.Session.SetInt32("id", author.Id);

                return RedirectToAction("Index", "Blog");
            }
        }

    }
}
