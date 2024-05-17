using AutoMapper;
using Basecode.Data;
using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using Basecode.Main.Models;
using Basecode.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;

namespace Basecode.Main.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        INewEnrolleeService _newEnrolleeService;
        IUsersService _usersService;
        IRTPService _rtpService;
        IParentService _parentService;
        ITeacherService _teacherService;
        SignInManager<IdentityUser> _signInManager;
        public HomeController(ILogger<HomeController> logger, 
            IUsersService usersService,IMapper mapper,
            INewEnrolleeService newEnrolleeService, 
            IRTPService rTPService,
            IParentService parentService,
            ITeacherService teacherService,
            SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _newEnrolleeService = newEnrolleeService;
            _usersService = usersService;
            _mapper = mapper;
            _rtpService = rTPService;
            _parentService = parentService;
            _teacherService = teacherService;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
            ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
            return View();
        }
        public IActionResult Enroll()
        {
            ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
            ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterEnrollee(RegisterStudent registerStudent)
        {          
            try
            {           
                _newEnrolleeService.RegisterStudent(registerStudent);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Your registration is now being processed. Please wait for an email with updates.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                Console.WriteLine(ex.Message);
                return RedirectToAction("Enroll");
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult RegisterTeacher(TeacherRegistrarionViewModel teacher)
        {
            try
            {
                _teacherService.AddTeacherRegistration(teacher);
                ViewBag.ErrorMessage = "Success";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                Console.WriteLine(ex.Message);
                return RedirectToAction("Enroll");
            }
        }
        public IActionResult LogIn()
        {
            try
            {
                if(!_signInManager.IsSignedIn(User))
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }
                else if(User.IsInRole("Registrar"))
                {
                    return RedirectToAction("Index", "Registrar");
                }
                else if(User.IsInRole("Teacher"))
                {
                    return RedirectToAction("Index", "Teacher");
                }
                else if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index");
            }
        }
    }
}