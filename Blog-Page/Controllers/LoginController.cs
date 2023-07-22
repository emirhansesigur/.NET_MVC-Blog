using Microsoft.AspNetCore.Mvc;

namespace BlogNET.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
