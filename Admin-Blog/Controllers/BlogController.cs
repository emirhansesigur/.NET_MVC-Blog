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

            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            //model.Category.Name = 

            if (model.CoverFoto != null)
            {
                string folder = "/images/";
                folder += model.CoverFoto.FileName + Guid.NewGuid().ToString();
                string serverFolder = _webHostEnvironment.WebRootPath + folder;

                await model.CoverFoto.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                model.AuthorId = (int)HttpContext.Session.GetInt32("id");

                await _context.AddAsync(model);
                await _context.SaveChangesAsync();
                return Json(true);
            
            }


            return Json(false);
        }

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
