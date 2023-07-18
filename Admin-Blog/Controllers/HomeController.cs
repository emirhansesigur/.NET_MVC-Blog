using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AdminBlog.Models;
using AdminBlog.Filter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Text;

namespace AdminBlog.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogContext _context; // veri tabanı context sınıfını tanıtıyoruz.
                                               // "readonly" anahtar kelimesi ise bir değişkenin sadece başlatma anında değer alabileceğini ve sonradan değiştirilemeyeceğini

        //private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, BlogContext context/*, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager*/)
        {
            _logger = logger;
            _context = context;
            //_signInManager = signInManager;
            //_userManager = userManager;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
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

                return RedirectToAction("Add","Blog");
            }
        }



        public async Task<IActionResult> AddCategory(Category category)
        {
            var sql = "SELECT * FROM Category WHERE Name = @Name";
            var count = _context.Category.FromSqlRaw(sql, new SqlParameter("@Name", category.Name)).ToList();

            if (!ModelState.IsValid)
            {
                //return View(category);
                return RedirectToAction(nameof(Category));
            }


            if (count.Count() == 0) // yoksa aramak icin
            {
                var sql2 = "INSERT INTO Category (Name) VALUES (@Name)";
                var affectedRows = await _context.Database.ExecuteSqlRawAsync(sql2, new SqlParameter("@Name", category.Name));
            }
            // varsa ekrana hata dondur.

            return RedirectToAction(nameof(Category));
        }

        //public async Task<IActionResult> AddAuthor(Author author) // ayni email ile oldugu zaman kabul etme.
        //{
        //    // hash

        //    var sql = "INSERT INTO Author VALUES (@Name, @Surname, @Email, @Password)";

        //    var parameters = new List<SqlParameter>();
        //    parameters.Add(new SqlParameter("@Name", author.Name));
        //    parameters.Add(new SqlParameter("@Surname", author.Surname));
        //    parameters.Add(new SqlParameter("@Email", author.Email));
        //    parameters.Add(new SqlParameter("@Password", author.Password));
        //    parameters.Add(new SqlParameter("@Id", author.Id));

        //    await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        //    var list = _context.Author.FromSqlRaw("SELECT * FROM Author").ToList();
        //    return View(list);
        //}

        public async Task<IActionResult> AddAuthor(Author author)
        {
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
                var list = _context.Author.FromSqlRaw("SELECT * FROM Author").ToList();
                return View(list);
            }

        }

        public async Task<IActionResult> AuthorDetails(int Id) // ?
        {

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


        public async Task<IActionResult> CategoryDetails(int Id) // bakilacak
        {
            var category = await _context.Category.FindAsync(Id);
            return Json(category);
        }

        //[Authorize]
        public IActionResult Category()
        {
            var list = _context.Category.FromSqlRaw("SELECT * FROM Category").ToList();
            return View(list);
        }


        //public IActionResult Author()
        //{
        //    List<Author> list = _context.Author.ToList();
        //    return View(list);
        //}

        //[Authorize]
        public IActionResult Author()
        {
            var list = _context.Author.FromSqlRaw("SELECT * FROM Author").ToList();
            return View(list);
        }



        //<a class="btn btn-danger" asp-route-id="@item.Id" asp-action="DeleteCategory">Sil</a>
        public async Task<IActionResult> DeleteCategory(int? Id)
        {
            var sql = "DELETE FROM Category WHERE Id = @Id";
            await _context.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@Id", Id));


            return RedirectToAction(nameof(Category));
        } //ExecuteSqlRawAsync: Bu, _context.Database üzerinde yer alan bir metoddur ve verilen SQL ifadesini doğrudan veritabanında yürütür. 
          //@Id yerine geçerli Id değerini kullanmayı sağlar. Bu şekilde, SQL enjeksiyon saldırılarına karşı güvenli bir şekilde çalışmayı sağlar.

        public async Task<IActionResult> UpdateCategory(int? Id, string Name) // burada kaldık
        {
            // eski kayıtlarla ile aynıysa güncelleME !!
            var sql = "SELECT * FROM Category WHERE Name = @Name";
            var count = _context.Category.FromSqlRaw(sql, new SqlParameter("@Name", Name)).ToList();


            if (count.Count() == 0) // yoksa guncellemek icin
            {
                var sqll = "UPDATE Category SET Name = @Name WHERE Id = @Id";
                await _context.Database.ExecuteSqlRawAsync(sqll, new SqlParameter("@Id", Id), new SqlParameter("@Name", Name));
            }

            else // burada view e AYNISINDAN VAR mesajİ yazdir. 
            { }

            return RedirectToAction(nameof(Category));
        }

        public async Task<IActionResult> UpdateAuthor(Author author) // burada kaldık
        {
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




        public async Task<IActionResult> DeleteAuthor(int? Id)
        {
            var sql = "DELETE FROM Author WHERE Id = @Id";
            await _context.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@Id", Id));

            return RedirectToAction(nameof(Author));
        }


        public IActionResult LogOut()
        {

            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }

        
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
