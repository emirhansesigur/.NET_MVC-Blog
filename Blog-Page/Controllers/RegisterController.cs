using Microsoft.AspNetCore.Mvc;

namespace BlogNET.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
