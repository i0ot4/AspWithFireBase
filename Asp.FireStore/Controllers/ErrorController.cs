using Microsoft.AspNetCore.Mvc;

namespace Asp.FireStore.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        public IActionResult NotFound404()
        {
            return View();
        }
    }
}
