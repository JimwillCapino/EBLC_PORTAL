using Basecode.Data;
using Basecode.Data.Models;
using Basecode.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Web;
//using System.Web.Mvc;

namespace Basecode_WebApp.Controllers
{
    [Authorize(Roles = "Registrar")]
    public class RegistrarController : Controller
    {
        private INewEnrolleeService _newEnrolleeService;
        private ITeacherService _teacherService;
        private ISubjectService _subjectService;
        private IClassManagementService _classManagementService;
        private IStudentManagementService _studentManagementService;
        private ISettingsService _settingsService;
        
        public RegistrarController(INewEnrolleeService newEnrolleeService,
            ITeacherService teacherService,
            ISubjectService subjectService,
            IClassManagementService classManagementService,
            IStudentManagementService studentManagementService,
            ISettingsService settingsService) 
        { 
            _newEnrolleeService = newEnrolleeService;
            _teacherService = teacherService;
            _subjectService = subjectService;
            _classManagementService = classManagementService;
            _studentManagementService = studentManagementService;
            _settingsService = settingsService;           
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult StudentRecord()
        {
            try
            {
                var studentList = _studentManagementService.GetAllStudents();
                return View(studentList);
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Index");
            }
        }
        public IActionResult StudentInfo(int student_Id, string school_year)
        {
            try
            {              
                if (school_year == null)
                {
                    var schoolYear = _settingsService.GetSettings().StartofClass.Value.Year.ToString() + "-" +
                _settingsService.GetSettings().EndofClass.Value.Year.ToString();
                    school_year = schoolYear;
                }
                   
                return View(_studentManagementService.GetStudentGrades(student_Id, school_year));
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Index");
            }         
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
        public IActionResult AdmitNewEnrollee(int id)
        {
            try
            {
                _newEnrolleeService.AdmitNewEnrollee(id);
                ViewBag.Success = true;
                return RedirectToAction("Enrollment");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Enrollee");
            }
        }
        public async Task<IActionResult> ManageTeachersAsync()
        {
            try
            {
                var teachers = await _teacherService.GetTeacherinitView();
                return View(teachers);
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public IActionResult ManageSubjects()
        {
            return View(_subjectService.GetSubjects());
        }

        [HttpPost]
        public IActionResult AddSubject()
        {
            try
            {
                var subname = Request.Form["name"];
                var grade = Int32.Parse(Request.Form["grade"]);
                var subject = new Subject {
                    Subject_Name = subname,
                    Grade = grade
                };

                _subjectService.AddSubject(subject);
                return RedirectToAction("ManageSubjects");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> ManageClass()
        {
            var classes = await _classManagementService.GetAllClass();
            return View(classes);
        }
        [HttpPost]
        public IActionResult AddClass()
        {
            try
            {
                string classname = Request.Form["className"];
                string adviser = Request.Form["Adviser"];
                int grade = Int32.Parse(Request.Form["grade"]);
                int classSize = Int32.Parse(Request.Form["classsize"]);

                var inputclass = new Class
                {
                    ClassName = classname,
                    Adviser = adviser,
                    ClassSize = classSize,
                    Grade = grade
                };

                _classManagementService.AddClass(inputclass);
                return RedirectToAction("ManageClass");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> ClassDetails(int classId)
        {
            try
            {
                var classdetails = await _classManagementService.GetClassViewModelById(classId);
                return View(classdetails);
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("ManageClass");
            }
        }
        [HttpPost]
        public IActionResult AddClassSubjects()
        {
            try
            {
                var classId = Int32.Parse(Request.Form["classId"]);
                var subjectId = Int32.Parse(Request.Form["subjectId"]);
                var teacherId = Request.Form["teacherId"];

                var classSubjects = new ClassSubjects
                {
                    ClassId = classId,
                    Subject_Id = subjectId,
                    Teacher_Id = teacherId
                };

                _classManagementService.AddClassSubjects(classSubjects);

                return RedirectToAction("ClassDetails", new { classId = classId });
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("ManageClass");
            }
        }
        [HttpPost]
        public IActionResult AddClassStudents()
        {
            try
            {
                var classId = Int32.Parse(Request.Form["classId"]);
                var studentid = Int32.Parse(Request.Form["studentId"]);
                var classStudent = new ClassStudents
                {
                    Class_Id = classId,
                    Student_Id = studentid,
                };
                _classManagementService.AddClassStudent(classStudent);
                return RedirectToAction("ClassDetails", new { classId = classId });
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("ManageClass");
            }
        }
        public IActionResult RemoveClassStudent(int id, int classId)
        {
            try
            {
                _classManagementService.RemoveClassStudent(id);
                return RedirectToAction("ClassDetails", new { classId = classId });
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("ManageClass");
            }
        }
        public IActionResult RemoveClassSubject(int id, int classId)
        {
            try
            {
                _classManagementService.RemoveClassSubject(id);
                return RedirectToAction("ClassDetails", new { classId = classId });
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("ManageClass");
            }
        }
        public IActionResult RemoveClass(int classId)
        {
            try
            {
                _classManagementService.RemoveClass(classId);
                return RedirectToAction("ManageClass");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("ManageClass");
            }
        }
        public IActionResult Settings()
        {
            return View(_settingsService.GetSettings);
        }
        public IActionResult UpdateSettings(Settings settings)
        {
            try
            {
                _settingsService.UpdateSettings(settings);
                return RedirectToAction("Settings");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public JsonResult GetCurrentSchoolYear()
        {
            var start = _settingsService.GetSettings().StartofClass;
            var end = _settingsService.GetSettings().EndofClass;
            string currentSchoolYear = start.Value.Year.ToString()+"-"+end.Value.Year.ToString(); // Replace this with your actual logic

            return Json(currentSchoolYear,System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }
    }
}
