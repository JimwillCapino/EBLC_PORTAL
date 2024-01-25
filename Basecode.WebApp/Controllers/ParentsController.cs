using Microsoft.AspNetCore.Mvc;

namespace Basecode_WebApp.Controllers
{
    public class ParentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
