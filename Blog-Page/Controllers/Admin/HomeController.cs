using Microsoft.AspNetCore.Mvc;

namespace BlogNET.Controllers.Admin
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
