using Microsoft.AspNetCore.Mvc;

namespace BlogNET.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Forbidden()
        {
            return View();
        }
    }
}
