using BlogNET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BlogNET.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogDbContext _context;

        public BlogController(BlogDbContext context)
        {
            _context = context;
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
            ViewBag.categoryNames = categoryNames.GetRange(0, 3);

            var blog = _context.Blog.Find(Id);
            //Burada
            return View(blog);
            //return View();
            // üstteki sorgu ile List gönderiyoruz ama 1 tek blog göndereceğiz. bunun detayına bak.
        }
    }
}
