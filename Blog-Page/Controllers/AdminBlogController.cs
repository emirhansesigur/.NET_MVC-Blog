﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using BlogNET.Models;

namespace BlogNET.Controllers
{
    public class AdminBlogController : Controller
    {
        private readonly ILogger<AdminBlogController> _logger;
        private readonly BlogDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminBlogController(ILogger<AdminBlogController> logger,
            BlogDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        //[Authorize(Roles = "SuperAdmin, Admin")]
        public IActionResult Index() // tum bloglar listelenir
        {
            var id = HttpContext.Session.GetInt32("Id");


            if (HttpContext.Session.GetString("superAdmin") != "superAdmin" && id == null)
            {
                //var list = _context.Blog.ToList();    
                return RedirectToAction("Forbidden", "Error");
            }

            List<Blog> list;

            if (HttpContext.Session.GetString("superAdmin") == "superAdmin")
            {
                list = _context.Blog.ToList();
            }
            else
            {
                list = _context.Blog.Where(blog => blog.AuthorId == id).ToList(); //Admin ise bu kullanılır.
            }

            return View(list);
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


        //[Authorize(Roles = "SuperAdmin")]
        public IActionResult BlockFun(int Id) // yayinlama islemini tersine cevirir.
        {
            if (HttpContext.Session.GetString("superAdmin") != "superAdmin")
            {
                //var list = _context.Blog.ToList();    
                return RedirectToAction("Forbidden", "Error");
            }

            var blog = _context.Blog.Find(Id);

            blog.IsBlocked = !(blog.IsBlocked); // istek gelince tersine cevir
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
            //model.CategoryName = foundCategory.Name;
            //if (!ModelState.IsValid) burada return View(model) yapamadığımız için validation kısmında sorun yaşanıyor.
            //{
            //    return View(model);
            //}

            if (model.CoverFoto != null)
            {
                // category name ekleme
                model.IsPublish = false;

                string parentDirectory = Directory.GetParent(_webHostEnvironment.ContentRootPath).FullName;

                string folder = "images/";
                folder += Guid.NewGuid().ToString() + model.CoverFoto.FileName;

                string serverFolder = parentDirectory + "/Blog-Page/wwwroot/" + folder;

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
            if (HttpContext.Session.GetString("superAdmin") != "superAdmin" || id == null)
            {
                //var list = _context.Blog.ToList();    
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

    }

}
