using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Basecode.Services.Interfaces;
namespace Basecode.WebApp.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IClassManagementService _classManagementService;
        public TeacherController(UserManager<IdentityUser> userManager, 
            IClassManagementService classManagementService)
        {
            _userManager = userManager;
            _classManagementService = classManagementService;
        }
        public IActionResult Index()
        {
            //var id = _userManager.GetUserId(User);
            return View();
        }
        public IActionResult StudentList()
        {
            var id = _userManager.GetUserId(User);
            var ClassList = _classManagementService.GetTeacherClassDetails(id);
            return View(ClassList);
        }
        public IActionResult SubmitGrade()
        {
            
            return View();
        }
    }
}
