﻿using AdminBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace AdminBlog.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ILogger<CategoryController> _logger;
        private readonly BlogContext _context; // veri tabanı context sınıfını tanıtıyoruz.
                                               // "readonly" anahtar kelimesi ise bir değişkenin sadece başlatma anında değer alabileceğini ve sonradan değiştirilemeyeceğini

        //private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly UserManager<IdentityUser> _userManager;

        public CategoryController(ILogger<CategoryController> logger, BlogContext context/*, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager*/)
        {
            _logger = logger;
            _context = context;
            //_signInManager = signInManager;
            //_userManager = userManager;
        }


        public IActionResult Index()
        {
            var list = _context.Category.FromSqlRaw("SELECT * FROM Category").ToList();
            return View(list);
        }


        public async Task<IActionResult> AddCategory(Category category)
        {

            if (!ModelState.IsValid)
            {
                //return View(category);
                return RedirectToAction(nameof(Index));
            }


            var sql = "SELECT * FROM Category WHERE Name = @Name";
            var count = _context.Category.FromSqlRaw(sql, new SqlParameter("@Name", category.Name)).ToList();



            if (count.Count() == 0) // yoksa aramak icin
            {
                var sql2 = "INSERT INTO Category (Name) VALUES (@Name)";
                var affectedRows = await _context.Database.ExecuteSqlRawAsync(sql2, new SqlParameter("@Name", category.Name));
            }
            // varsa ekrana hata dondur.

            return RedirectToAction(nameof(Index));
        }

        //[Authorize]
        public IActionResult Category()
        {
            var list = _context.Category.FromSqlRaw("SELECT * FROM Category").ToList();

            return View(list);
        }

        public async Task<IActionResult> CategoryDetails(int Id) // bakilacak
        {
            var category = await _context.Category.FindAsync(Id);
            return Json(category);
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


    }
}
