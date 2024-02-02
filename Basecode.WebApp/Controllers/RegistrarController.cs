using Basecode.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Basecode_WebApp.Controllers
{
    public class RegistrarController : Controller
    {
        private INewEnrolleeService _newEnrolleeService;
        public RegistrarController(INewEnrolleeService newEnrolleeService) 
        { 
            _newEnrolleeService = newEnrolleeService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult StudentRecord()
        {
            return View();
        }
        public IActionResult StudentInfo()
        {
            return View();
        }
        public IActionResult MonthlyFee()
        {
            return View();
        }
        public IActionResult Enrollment()
        {
            return View(_newEnrolleeService.GetAllEnrollees());
        }
    }
}
