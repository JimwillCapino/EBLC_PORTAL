using Basecode.Data;
using Basecode.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Web;
namespace Basecode_WebApp.Controllers
{
    [Authorize(Roles = "Registrar")]
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
            try
            {
                return View(_newEnrolleeService.GetNewEnrolleeInitView());
            }
            catch
            {
                ViewBag.ErrorMessage = Constants.Exception.DB;
                return RedirectToAction("Index");
            }
        }
        public IActionResult NewEnrolleeInfo(int studentId)
        {
            try
            {
                return View(_newEnrolleeService.GetStudent(studentId));
            }
            catch
            {
                ViewBag.ErrorMessage = Constants.Exception.DB;
                return RedirectToAction("Enrollment");
            }
        }
      
        [HttpPost]
        public IActionResult AddSchedule()
        {
            var Schedule = Request.Form["datetime"];
            DateTime parseSched = DateTime.Parse(Schedule);
            int id = Int32.Parse(Request.Form["Id"]);
            try
            {
                _newEnrolleeService.AddSchedule(id, parseSched);
                ViewBag.Success=true;
                return RedirectToAction("Enrollment");
            }
            catch(Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("NewEnrolleeInfo",id);
            }
        }
        public IActionResult RejectNewEnrollee(int id)
        {
            try
            {
                _newEnrolleeService.RejectNewEnrollee(id);
                return RedirectToAction("Enrollment");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("NewEnrolleeInfo", id);
            }
        }
    }
}
