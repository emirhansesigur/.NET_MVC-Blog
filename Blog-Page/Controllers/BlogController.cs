using BlogNET.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogNET.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogController(BlogDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            //var categories = _context.Category.ToList();
            //ViewBag.categories = categories;

            var blogs = _context.Blog
                    .OrderByDescending(b => b.CreateTime)
                    .ToList();

            return View(blogs);
        }

        public IActionResult Details(int Id) // parametre olarak alma.
        {
            var categoryNames = _context.Category.Select(c => c.Name).ToList();
            //ViewBag.categoryNames = categoryNames.GetRange(0, 3);

            var blog = _context.Blog.Find(Id);
            //Burada
            return View(blog);
            //return View();
            // üstteki sorgu ile List gönderiyoruz ama 1 tek blog göndereceğiz. bunun detayına bak.
        }


        //[Authorize(Roles = "SuperAdmin, Admin")]
        public IActionResult Publish(int Id) // yayinlama islemini tersine cevirir.
        {
            var id = HttpContext.Session.GetInt32("Id");
            if (HttpContext.Session.GetString("superAdmin") != "superAdmin" && id == null)
            {
                //var list = _context.Blog.ToList();    
                return RedirectToAction("Forbidden", "Error");
            }

            var blog = _context.Blog.Find(Id);

            blog.IsPublish = !(blog.IsPublish);
            _context.Update(blog);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        //[Authorize(Roles = "Admin")]
        public IActionResult Add() // Categories'e gonderiliyor.
        {
            var id = HttpContext.Session.GetInt32("Id");
            if (id == null)
            {
                //var list = _context.Blog.ToList();    
                return RedirectToAction("Forbidden", "Error");
            }

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



        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(Blog model) // Category.Id elimizde. 
        {
            var id = HttpContext.Session.GetInt32("Id");
            if (id == null)
            {
                //var list = _context.Blog.ToList();    
                return RedirectToAction("Forbidden", "Error");
            }

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

            if (model.CoverFoto != null)
            {
                // category name ekleme
                model.IsPublish = false;

                string ContentRootPath = _webHostEnvironment.ContentRootPath;

                string folder = "images/";
                folder += Guid.NewGuid().ToString() + model.CoverFoto.FileName;

                string serverFolder = ContentRootPath + "/wwwroot/" + folder;

                model.ImagePath = folder;


                await model.CoverFoto.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                model.AuthorId = (int)HttpContext.Session.GetInt32("Id");

                await _context.AddAsync(model);
                await _context.SaveChangesAsync();
                return Json(true);
            }


            return Json(false);
        }



        //[Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> DeleteBlog(int id2)
        {
            var id = HttpContext.Session.GetInt32("Id");
            if (HttpContext.Session.GetString("superAdmin") != "superAdmin" && id == null)
            {
                return RedirectToAction("Forbidden", "Error");
            }

            var blogToDelete = await _context.Blog.FindAsync(id2);

            if (blogToDelete == null)
            {
                // Blog bulunamazsa HTTP 404 (Not Found) döndürün
                return NotFound();
            }

            // Blog bulunduğunda doğrudan silme işlemini gerçekleştiririz.
            _context.Blog.Remove(blogToDelete);
            

            // Değişiklikleri veritabanına kaydedin
            await _context.SaveChangesAsync();
            // Silme işlemi başarılı olduğunda HTTP 200 (OK) döndürün
            return Ok(new { success = true });
        }

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Comment()
        {
            var id = HttpContext.Session.GetInt32("Id");
            if (id == null)
            {
                return RedirectToAction("Forbidden", "Error");
            }




            await _context.SaveChangesAsync();
            return Json(true);
        }

        public IActionResult Content()
        {
            return View();
        }


    }



}

