using AutoMapper;
using Basecode.Data;
using Basecode.Data.Models;
using Basecode.Main.Models;
using Basecode.Services.Interfaces;
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
        
        public HomeController(ILogger<HomeController> logger, IUsersService usersService,IMapper mapper,INewEnrolleeService newEnrolleeService)
        {
            _logger = logger;
            _newEnrolleeService = newEnrolleeService;
            _usersService = usersService;
            _mapper = mapper;
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
        [HttpPost]
        public IActionResult RegisterEnrollee(RegisterStudent registerStudent)
        {
            try
            {
                Constants.Enrollee.id = _usersService.AddUser(_mapper.Map<UsersPortal>(registerStudent));
                _newEnrolleeService.RegisterStudent(registerStudent);
                ViewBag.ErrorMessage = null;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                Console.WriteLine(ex.Message);
                return RedirectToAction("Enroll");
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}