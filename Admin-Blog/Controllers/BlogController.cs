using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AdminBlog.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using AdminBlog.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace AdminBlog.Controllers
{
	//[UserFilter] - [authorize değil miydi ?]
	// buraya bak.
	public class BlogController : Controller
	{
		private readonly ILogger<BlogController> _logger;
		private readonly BlogContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public BlogController(ILogger<BlogController> logger,
			BlogContext context,
			IWebHostEnvironment webHostEnvironment)
		{
			_logger = logger;
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		//[Authorize]
		public IActionResult Index()
		{
			//var list = _context.Blog.ToList();
			var list = _context.Blog.FromSqlRaw("SELECT * FROM Blog").ToList();

			return View(list);
		}

		public IActionResult Publish(int Id) // yayinlama islemini tersine cevirir.
		{
			var blog = _context.Blog.Find(Id);

			blog.IsPublish = !(blog.IsPublish);
			_context.Update(blog);
			_context.SaveChanges();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Add() // Categories'e gonderiliyor.
		{
			List<SelectListItem> values = _context.Category.Select(w =>
				new SelectListItem
				{
					Text = w.Name,
					Value = w.Id.ToString()
				}
			).ToList();
			ViewBag.Values = values;
			return View();
		}

		[HttpPost]
		//public IActionResult Add(Blog model) 
		public async Task<IActionResult> Add(Blog model) // Category.Id elimizde. 
		{
			// Category ismini de elde etmemiz gerekiyor.
			var foundCategory = _context.Category.Find(model.CategoryId);

			if (foundCategory == null)
			{
				return Json(false); // false yerine baska bir sey dondurerek uyarı verdir.
			}

			// Eğer nesne null değilse categoryName özelliğine erişme
			model.CategoryName = foundCategory.Name;
			//if (!ModelState.IsValid) burada return View(model) yapamadığımız için validation kısmında sorun yaşanıyor.
			//{
			//    return View(model);
			//}

			//model.Category.Name = 

			if (model.CoverFoto != null)
			{
				// category name ekleme
				model.IsPublish = false;

				string parentDirectory = Directory.GetParent(_webHostEnvironment.ContentRootPath).FullName;
				
				string folder = "images/";
				folder += Guid.NewGuid().ToString() +  model.CoverFoto.FileName;
				
				string serverFolder = parentDirectory + "/Blog-Page/wwwroot/" + folder;

				model.ImagePath = folder;


                await model.CoverFoto.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
				model.AuthorId = (int)HttpContext.Session.GetInt32("id");

				await _context.AddAsync(model);
				await _context.SaveChangesAsync();
				return Json(true);

			}


			return Json(false);
		}

        //string parentDirectory = Directory.GetParent(_webHostEnvironment.ContentRootPath).FullName;

        //string folder = "/Blog-Page/wwwroot/images/";
        //folder += model.CoverFoto.FileName + Guid.NewGuid().ToString();
        //string serverFolder = parentDirectory + folder;	

        //[HttpPost]
        //public async Task<IActionResult> Add(Blog model)
        //{ //C:\Users\Emir\Desktop\admin-blog-master
        //    if (model != null){
        //        if (Request.Form.Files.Count > 0) //en az bir dosya olduğundan emin olursunuz ve dosya işleme işlemine devam edebilirsiniz.
        //        {
        //            var file = Request.Form.Files.First(); //C:\Users\Emir\Desktop\admin-blog-master\wwwroot\images
        //            string savePath = Path.Combine("C:", "Users", "Emir", "Desktop", "admin-blog-master", "wwwroot", "images");
        //            var fileName = $"{DateTime.Now:MMddHHmmss}.{file.FileName.Split(".").Last()}";
        //            var fileUrl = Path.Combine(savePath, fileName);
        //            using (var fileStream = new FileStream(fileUrl, FileMode.Create))
        //            {
        //                await file.CopyToAsync(fileStream);
        //            }
        //            model.ImagePath = fileName;
        //            model.AuthorId = (int)HttpContext.Session.GetInt32("id");
        //            await _context.AddAsync(model);
        //            await _context.SaveChangesAsync();
        //            return Json(true);
        //        }
        //    }

        //    return Json(false);
        //}


        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blogToDelete = await _context.Blog.FindAsync(id);

            if (blogToDelete == null)
            {
                // Blog bulunamazsa HTTP 404 (Not Found) döndürün
                return NotFound();
            }

            // Blog bulunduğunda doğrudan silme işlemini gerçekleştiririz.
            _context.Blog.Remove(blogToDelete);

            // Değişiklikleri veritabanına kaydedin
            try
            {
                await _context.SaveChangesAsync();
                // Silme işlemi başarılı olduğunda HTTP 200 (OK) döndürün
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                // Hata durumunda HTTP 500 (Internal Server Error) döndürün ve hata mesajını içeren bir nesne döndürün
                return StatusCode(500, new { success = false, message = "Silme işlemi başarısız oldu." });
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
