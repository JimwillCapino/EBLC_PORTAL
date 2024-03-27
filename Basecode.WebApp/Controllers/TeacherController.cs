using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Basecode.Services.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
namespace Basecode.WebApp.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IClassManagementService _classManagementService;
        private readonly IStudentManagementService _studentManagementService;
        private readonly ISettingsService _settingsService;
        private readonly ISubjectService _subjectService;
        private readonly IStudentService _studentService;
        public TeacherController(UserManager<IdentityUser> userManager, 
            IClassManagementService classManagementService,
            IStudentManagementService studentManagementService,
            ISettingsService settingsService,
            ISubjectService subjectService,
            IStudentService studentService)
        {
            _userManager = userManager;
            _classManagementService = classManagementService;
            _studentManagementService = studentManagementService;
            _settingsService = settingsService;
            _subjectService = subjectService;
            _studentService = studentService;
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
            var subject = _subjectService.GetSubject(subject_Id);

            if(subject.HasChild)
            {
                return RedirectToAction("ChildSubjectGrades", new { headId = subject_Id, studentId = student_Id });
            }
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
        public IActionResult HomeRoom()
        {
            var id = _userManager.GetUserId(User);
            return View(_classManagementService.GetTeacherHomeRoom(id));
        }
        public IActionResult StudentValues(int studentId, string school_year)
        {
            try
            {
                if (school_year == null)
                {
                    var schoolYear = _settingsService.GetSettings().StartofClass.Value.Year.ToString() + "-" +
                _settingsService.GetSettings().EndofClass.Value.Year.ToString();
                    school_year = schoolYear;
                }
                var test = _studentManagementService.GetValuesWithGrades(studentId, school_year);
                return View(test);                
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult SubmitGradeValues()
        {
            try
            {
                var behaviouralId = Int32.Parse(Request.Form["Behavioural"]);
                var quarter = Int32.Parse(Request.Form["Quarter"]);
                var School_Year = Request.Form["School_Year"];
                var Student_Id = Int32.Parse(Request.Form["Student_Id"]);
                var grades = Request.Form["Grades"];

                var values = new Learner_Values
                {
                    Behavioural_Statement = behaviouralId,
                    Quarter = quarter,
                    School_Year = School_Year,
                    Student_Id = Student_Id,
                    Grade = grades
                };
                _studentManagementService.AddLearnerValues(values);

                return RedirectToAction("StudentValues", new { studentId = Student_Id , school_year = School_Year });
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult UpdateLearnerValues()
        {
            try
            {
                var id = Int32.Parse(Request.Form["LearnerValuesId"]);
                var grade = Request.Form["Grades"];
                var studentId = Int32.Parse(Request.Form["StudentId"]);
                _studentManagementService.UpdateLearnerValues(id, grade);
                return RedirectToAction("StudentValues",new {studentId = studentId });
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Index");
            }
        }
        public IActionResult ChildSubjectGrades(int headId, int studentId)
        {
            try

            {
                return View(_studentManagementService.GetChildSubjectGrades(headId, studentId));
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult AddChildSubjectGrades()
        {
            try
            {
                double headSubjectAverage = 0;
                double sum = 0;
                var HeadId = Int32.Parse(Request.Form["headId"]);
                var studentId = Int32.Parse(Request.Form["studentId"]);
                var subjects = Request.Form["subject"];
                var grades = Request.Form["grades"];
                var quarter = Int32.Parse(Request.Form["quarter"]);
                var gradeLevel = _studentService.GetStudent(studentId).CurrGrade;                

                for(int x=0; x<subjects.Count;x++)
                {
                    var subjectId = Int32.Parse(subjects[x]);
                    var grade = Int32.Parse(grades[x]);
                    sum += grade;
                    _studentManagementService.SubmitGrade(studentId, subjectId, grade, quarter);
                }
                headSubjectAverage = Math.Floor(sum / subjects.Count);
                _studentManagementService.SubmitGrade(studentId, HeadId, (int)headSubjectAverage, quarter);               
                if(quarter == 4)
                {
                    var suminner = 0;
                    double avg = 0;
                    var MainSubjectGrade = _studentManagementService.GetStudentGradeBySubject(studentId, HeadId);
                    foreach(var grade in MainSubjectGrade.Grades) 
                    {
                        suminner += grade.Grade;
                    }
                    avg = suminner / MainSubjectGrade.Grades.Count;
                    _studentManagementService.SubmitGrade(studentId, HeadId, (int)avg, quarter+1);
                }
                return RedirectToAction("ChildSubjectGrades", new { headId = HeadId, studentId = studentId });
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Index");
            }
        }
        public IActionResult AddStudentAttendance(AttendanceContainer container)
        {
            try
            {
                _studentManagementService.AddStudentAttendance(container.studentId, container.Days_of_School, container.Days_of_Present, container.Time_of_Tardy, container.Month);
                return RedirectToAction("StudentAttendance", new { studentId = container.studentId });    
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                ViewBag.Success = false;
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Index");
            }
        }
        public IActionResult StudentAttendance(int studentId)
        {
            try
            {
                var schoolYear = _settingsService.GetSchoolYear();
                var container = _studentManagementService.GetStudentAttendance(studentId,schoolYear);
                return View(container);
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Index");
            }
        }
        public IActionResult UpdateStudentAttendance(AttendanceContainer container)
        {
            try
            {
                _studentManagementService.UpdateAttendance(container.Id, container.studentId, container.Days_of_School, container.Days_of_Present, container.Time_of_Tardy, container.Month);
                return RedirectToAction("StudentAttendance", new { studentId = container.studentId });
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
