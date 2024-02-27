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
        private readonly IStudentManagementService _studentManagementService;
        public TeacherController(UserManager<IdentityUser> userManager, 
            IClassManagementService classManagementService,
            IStudentManagementService studentManagementService)
        {
            _userManager = userManager;
            _classManagementService = classManagementService;
            _studentManagementService = studentManagementService;
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
        public IActionResult SubmitGrade(int student_Id, int subject_Id)
        {
            var grades = _studentManagementService.GetStudentGradeBySubject(student_Id, subject_Id);
            return View(grades);
        }
        [HttpPost]
        public IActionResult AddGrade()
        {
            var student_id = Int32.Parse(Request.Form["Student_Id"]);
            var subject_Id = Int32.Parse(Request.Form["Subject_Id"]);
            var grade = Int32.Parse(Request.Form["Grade"]);
            var quarter = Int32.Parse(Request.Form["Quarter"]);

            _studentManagementService.SubmitGrade(student_id, subject_Id, grade, quarter);
            return RedirectToAction("SubmitGrade", new { student_Id = student_id , subject_Id = subject_Id });
        }
        [HttpPost]
        public IActionResult EditGrade()
        {
            var Grade_Id = Int32.Parse(Request.Form["Grade_Id"]);
            var student_id = Int32.Parse(Request.Form["Student_Id"]);
            var subject_Id = Int32.Parse(Request.Form["Subject_Id"]);
            var grade = Int32.Parse(Request.Form["Grade"]);
            var quarter = Int32.Parse(Request.Form["Quarter"]);

            _studentManagementService.EditGrade(Grade_Id, student_id, subject_Id, grade, quarter);
            return RedirectToAction("SubmitGrade", new { student_Id = student_id, subject_Id = subject_Id });
        }
    }
}
