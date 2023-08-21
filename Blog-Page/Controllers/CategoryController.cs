using BlogNET.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNET.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ILogger<CategoryController> _logger;
        private readonly BlogDbContext _context; // veri tabanı context sınıfını tanıtıyoruz.
                                                 // "readonly" anahtar kelimesi ise bir değişkenin sadece başlatma anında değer alabileceğini ve sonradan değiştirilemeyeceğini

        public CategoryController(ILogger<CategoryController> logger, BlogDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        //[Authorize(Roles = "SuperAdmin")]
        public IActionResult Index()
        {
            //var id = HttpContext.Session.GetInt32("Id");
            var superadmin = HttpContext.Session.GetString("superAdmin");
            //if (id.HasValue)
            //{
            //    var categories = _context.Category
            //            .Where(category => category.AuthorId == id)
            //            .ToList();
            //    return View(categories);

            //}
            //else 
            if (superadmin == "superAdmin")
            {
                // eger superadmin giris yaptiysa hepsini dondur
                var categories = _context.Category.ToList();
                return View(categories);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            // eger admin giris yaptiysa sadece onun kategorilerini dondur
        }

        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddCategory(Category category)
        {

            //if (!ModelState.IsValid)
            //{
            //    //return View(category);
            //    return RedirectToAction(nameof(Index));
            //}


            var AuthorId = (int)HttpContext.Session.GetInt32("Id");
            string authorName = _context.Author
                        .Where(author => author.Id == AuthorId)
                        .Select(author => author.Name)
                        .FirstOrDefault();

            var sql = "SELECT * FROM Category WHERE Name = @Name";
            var count = _context.Category.FromSqlRaw(sql, new SqlParameter("@Name", category.Name)).ToList();



            if (count.Count() == 0) // yoksa aramak icin
            {
                var sql2 = "INSERT INTO Category (Name) VALUES (@Name)";
                var affectedRows = await _context.Database.ExecuteSqlRawAsync(sql2, new SqlParameter("@Name", category.Name));
            }
            // varsa ekrana hata dondur.

            return Json(true);
        }


        public IActionResult CategoryDetails(int Id) // bakilacak
        {
            //var category = await _context.Category.FindAsync(Id);
            var category = _context.Category.Find(Id);
            return Json(category);
        }

        //<a class="btn btn-danger" asp-route-id="@item.Id" asp-action="DeleteCategory">Sil</a>
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteCategory(int? Id)
        {
            var sql = "DELETE FROM Category WHERE Id = @Id";
            await _context.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@Id", Id));


            return RedirectToAction(nameof(Index));
        } //ExecuteSqlRawAsync: Bu, _context.Database üzerinde yer alan bir metoddur ve verilen SQL ifadesini doğrudan veritabanında yürütür. 
          //@Id yerine geçerli Id değerini kullanmayı sağlar. Bu şekilde, SQL enjeksiyon saldırılarına karşı güvenli bir şekilde çalışmayı sağlar.


        //[Authorize(Roles = "SuperAdmin")]
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

            return RedirectToAction(nameof(Index));
        }


    }
}
