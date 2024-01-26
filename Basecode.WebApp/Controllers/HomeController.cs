using Basecode.Data.Models;
using Basecode.Main.Models;
using Basecode.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Basecode.Main.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        INewEnrolleeService _newEnrolleeService;
        IUsersService _usersService;
        public HomeController(ILogger<HomeController> logger, INewEnrolleeService newEnrolleeService, IUsersService usersService)
        {
            _logger = logger;
            _newEnrolleeService = newEnrolleeService;
            _usersService = usersService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Enroll()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult RegisterEnrollee(RegisterStudent registerStudent)
        {
            Users user = new Users();
            NewEnrollee newEnrollee = new NewEnrollee();
            //User details
            user.FirstName = registerStudent.First_Name;
            user.MiddleName = registerStudent.Middle_Name;
            user.LastName = registerStudent.Last_Name;
            user.PhoneNumber = registerStudent.Phone_Number;
            user.email = registerStudent.Email;
            user.sex = registerStudent.gender;
            user.role = "student";

            //New enrollee information
            newEnrollee.BirthCertificate = registerStudent.birthCertificate;
            newEnrollee.CGM = registerStudent.COG;
            newEnrollee.TOR = registerStudent.TOR;

            int User = _usersService.AddUser(user);
            newEnrollee.UID = user.UID;
            int NewEnrollee = _newEnrolleeService.RegisterStudent(newEnrollee);

            if (User == -1)
            {
                ViewData["ErrorMessage"] = "Error Occured. Check the console.";
            }
            if(NewEnrollee == -1) 
            {
                ViewData["ErrorMessage"] = "Error Occured. Check the console.";
            }
            else
            {
                return RedirectToAction("Index");
            }
            
            return View(registerStudent);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}