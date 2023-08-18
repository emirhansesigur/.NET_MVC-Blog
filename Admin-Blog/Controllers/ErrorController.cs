using Microsoft.AspNetCore.Mvc;

namespace AdminBlog.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Forbidden()
        {
            return View();
        }
    }
}
