using Basecode.Data.ViewModels;
using Basecode.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Basecode.WebApp.Controllers
{
    public class AdminController : Controller
    {
        IAdminService _adminService;
        public AdminController(IAdminService adminService) 
        { 
            _adminService = adminService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel r)
        {           
            if(ModelState.IsValid)
            {
                IdentityResult result = await _adminService.CreateRole(r.Role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                Console.Write("Failed");
            }           
            Console.WriteLine("Model State invalid");
            return RedirectToAction("Index");
        }
    }
}
