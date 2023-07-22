using Microsoft.AspNetCore.Mvc;

namespace BlogNET.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
