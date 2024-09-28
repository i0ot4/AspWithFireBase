using Microsoft.AspNetCore.Mvc;

namespace Asp.FireStore.Controllers
{
    public class AdminInfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
