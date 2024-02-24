using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity; 
namespace Basecode.WebApp.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public TeacherController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            //var id = _userManager.GetUserId(User);
            return View();
        }
        public IActionResult StudentList()
        {
            return View();
        }
    }
}
