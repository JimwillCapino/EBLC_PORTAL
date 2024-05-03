using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Basecode.Services.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using Basecode.Data;
using Basecode.Data.Repositories;
using Basecode.Data.Interfaces;
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
        private readonly IUsersService _usersService;
        public TeacherController(UserManager<IdentityUser> userManager, 
            IClassManagementService classManagementService,
            IStudentManagementService studentManagementService,
            ISettingsService settingsService,
            ISubjectService subjectService,
            IStudentService studentService,
            IUsersService usersService)
        {
            _userManager = userManager;
            _classManagementService = classManagementService;
            _studentManagementService = studentManagementService;
            _settingsService = settingsService;
            _subjectService = subjectService;
            _studentService = studentService;
            _usersService = usersService;
        }
        public async Task<IActionResult> Index(int classid, int subjectid, int quarter)
        {
            ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
            ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
            var id = _userManager.GetUserId(User);
            //var id = _userManager.GetUserId(User);
            if (await _usersService.IsNewUser(_userManager.GetUserId(User)))
            {
                return RedirectToAction("TeacherProfileRegistration");
            }
            TeacherDashboard dashboard = new TeacherDashboard()
            {
                SchoolYear = _settingsService.GetSchoolYear(),
                SubjectName = _subjectService.GetSubject(subjectid) != null ? _subjectService.GetSubject(subjectid).Subject_Name : " ",
                NumberOfClass = _classManagementService.GetTeacherClassDetails(id).Count(),
                NumberOfHomeroom = _classManagementService.GetTeacherHomeRoom(id).Count(),
                ListOfStudentsWithNoGrade = _studentManagementService.GetStudentWithNoGradePerQuarter(classid, subjectid, quarter),
                ClassesOfTeacher = _classManagementService.GetTeacherClassDetails(id)
            };
            return View(dashboard);
        }
        [HttpPost]
        public IActionResult QueryDashboad()
        {
            try
            {
                var classattended = Request.Form["class"].ToString();
                var quarter = Int32.Parse(Request.Form["quarter"]);

                string[] numbersArray = classattended.Split(' ');
                int classid = Int32.Parse(numbersArray[0]);
                int subjectid = Int32.Parse(numbersArray[1]);
                return RedirectToAction("Index", new { classid = classid, subjectid = subjectid, quarter = quarter });
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> Profile()
        {
            try
            {
                ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
                ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
                return View(await _usersService.GetUserPortal(_userManager.GetUserId(User)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> UpdatePorfile(ProfileViewModel profile)
        {
            try
            {
                await _usersService.UpdateUserProfile(profile);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Profile Updated Successfully!";

                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                Console.WriteLine(ex);
                return RedirectToAction("Profile");
            }
        }
        public async Task<IActionResult> ChangePassword(ProfileViewModel profile)
        {
            try
            {
                await _usersService.ChangePassword(profile);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Passoword changed successfully!";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                Console.WriteLine(ex);
                return RedirectToAction("Profile");
            }
        }
        public IActionResult TeacherProfileRegistration()
        {
            ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
            ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
            return View();
        }
        public async Task <IActionResult> UserSetUpTeacher(ProfileViewModel profile)
        {
            try
            {
                profile.AspUserId = _userManager.GetUserId(User);
                await _usersService.NewUserDetailsRegistration(profile);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Teacher's Profile created successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                return RedirectToAction("TeacherProfileRegistration");
            }
        }
        public IActionResult StudentListPerSubject(int classid, int subjectid)
        {
            try
            {
                var teacher = _classManagementService.GetTeacherSubjectDetails(classid, subjectid);
                return View(teacher);
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                return RedirectToAction("StudentList");
            }
        }
        [HttpPost]
        public IActionResult StudentListPerSubjectDataTable(int classid)
        {
            try
            {
                var id = _userManager.GetUserId(User);
                var draw = int.Parse(Request.Form["draw"]);
                var start = int.Parse(Request.Form["start"]);
                var length = int.Parse(Request.Form["length"]);
                var searchValue = Request.Form["search[value]"];
                Constants.TeacherNavigation.classid = classid;
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                //Paging Size (10,25,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data  
                var customerData = _classManagementService.GetClassStudents(classid);
                var asEnumcustomerData = customerData.AsEnumerable();

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumn.Equals("firstname"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.firstname) :
                            asEnumcustomerData.OrderByDescending(p => p.firstname);
                    }
                    else if (sortColumn.Equals("middlename"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.middlename) :
                            asEnumcustomerData.OrderByDescending(p => p.middlename);
                    }
                    else if (sortColumn.Equals("lastname"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.lastname) :
                            asEnumcustomerData.OrderByDescending(p => p.lastname);
                    }

                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    asEnumcustomerData = customerData.Where(m => m.firstname.ToLower().Contains(searchValue.ToString().ToLower())
                      || m.middlename.ToLower().Contains(searchValue.ToString().ToLower()) || m.lastname.ToLower().Contains(searchValue.ToString().ToLower()));
                }


                //total number of rows count   
                recordsTotal = asEnumcustomerData.Count();
                //Paging   
                var data = asEnumcustomerData.Skip(skip).Take(pageSize);
                //Returning Json Data  
                var test = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                return test;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult StudentList()
        {
            //var id = _userManager.GetUserId(User);
            //var ClassList = _classManagementService.GetTeacherClassDetails(id);
            return View();
        }
        [HttpPost]
        public IActionResult StudentListDataTable()
        {
            try
            {
                var id = _userManager.GetUserId(User);
                var draw = int.Parse(Request.Form["draw"]);
                var start = int.Parse(Request.Form["start"]);
                var length = int.Parse(Request.Form["length"]);
                var searchValue = Request.Form["search[value]"];

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                //Paging Size (10,25,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data  
                var customerData = _classManagementService.GetTeacherClassDetails(id);
                var asEnumcustomerData = customerData.AsEnumerable();

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumn.Equals("classname"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.classname) :
                            asEnumcustomerData.OrderByDescending(p => p.classname);
                    }
                    else if (sortColumn.Equals("grade"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.grade) :
                            asEnumcustomerData.OrderByDescending(p => p.grade);
                    }
                    else if (sortColumn.Equals("subjectname"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.subjectname) :
                            asEnumcustomerData.OrderByDescending(p => p.subjectname);
                    }

                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    asEnumcustomerData = customerData.Where(m => m.classname.ToLower().Contains(searchValue.ToString().ToLower())
                      || m.grade.ToString().Contains(searchValue.ToString()) || m.subjectname.ToLower().Contains(searchValue.ToString().ToLower()));
                }

                //total number of rows count   
                recordsTotal = asEnumcustomerData.Count();
                //Paging   
                var data = asEnumcustomerData.Skip(skip).Take(pageSize);
                //Returning Json Data  
                var test = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                return test;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult SubmitGrade(int student_Id, int subject_Id)
        {
            var subject = _subjectService.GetSubject(subject_Id);

            if(subject.HasChild)
            {
                return RedirectToAction("ChildSubjectGrades", new { headId = subject_Id, studentId = student_Id });
            }
            var grades = _studentManagementService.GetStudentGradeBySubject(student_Id, subject_Id);
            grades.class_id = Constants.TeacherNavigation.classid;
            return View(grades);
        }
        [HttpPost]
        public IActionResult AddGrade(GradesDetail grade)
        {                               
            _studentManagementService.SubmitGrade(grade.Student_Id, grade.Subject_Id, grade.GradeInput, grade.Quarter);
            return RedirectToAction("SubmitGrade", new { student_Id = grade.Student_Id, subject_Id = grade.Subject_Id });
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
       
        [HttpPost]
        public  IActionResult HomeRoomDataTable()
        {
            try
            {
                var id = _userManager.GetUserId(User);
                var draw = int.Parse(Request.Form["draw"]);
                var start = int.Parse(Request.Form["start"]);
                var length = int.Parse(Request.Form["length"]);
                var searchValue = Request.Form["search[value]"];

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                //Paging Size (10,25,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data  
                var customerData = _classManagementService.GetTeacherHomeRoom(id);
                var asEnumcustomerData = customerData.AsEnumerable();

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumn.Equals("classname"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.classname) :
                            asEnumcustomerData.OrderByDescending(p => p.classname);
                    }
                    else if (sortColumn.Equals("grade"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.grade) :
                            asEnumcustomerData.OrderByDescending(p => p.grade);
                    }                 

                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    asEnumcustomerData = customerData.Where(m => m.classname.ToLower().Contains(searchValue.ToString().ToLower())
                      || m.grade.ToString().Contains(searchValue.ToString()));
                }

                //total number of rows count   
                recordsTotal = asEnumcustomerData.Count();
                //Paging   
                var data = asEnumcustomerData.Skip(skip).Take(pageSize);
                //Returning Json Data  
                var test = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                return test;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult HomeRoomStudents(int classid)
        {         
            return View(classid);
        }
        [HttpPost]
        public IActionResult HomeRoomStudentsDataTable(int classid)
        {
            try
            {
                var id = _userManager.GetUserId(User);
                var draw = int.Parse(Request.Form["draw"]);
                var start = int.Parse(Request.Form["start"]);
                var length = int.Parse(Request.Form["length"]);
                var searchValue = Request.Form["search[value]"];
                Constants.TeacherNavigation.classid = classid;
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                //Paging Size (10,25,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data  
                var customerData = _classManagementService.GetClassStudents(classid);
                var asEnumcustomerData = customerData.AsEnumerable();

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumn.Equals("firstname"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.firstname) :
                            asEnumcustomerData.OrderByDescending(p => p.firstname);
                    }
                    else if (sortColumn.Equals("middlename"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.middlename) :
                            asEnumcustomerData.OrderByDescending(p => p.middlename);
                    }
                    else if (sortColumn.Equals("lastname"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.lastname) :
                            asEnumcustomerData.OrderByDescending(p => p.lastname);
                    }

                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    asEnumcustomerData = customerData.Where(m => m.firstname.ToLower().Contains(searchValue.ToString().ToLower())
                      || m.middlename.ToLower().Contains(searchValue.ToString().ToLower()) || m.lastname.ToLower().Contains(searchValue.ToString().ToLower()));
                }

                //total number of rows count   
                recordsTotal = asEnumcustomerData.Count();
                //Paging   
                var data = asEnumcustomerData.Skip(skip).Take(pageSize);
                //Returning Json Data  
                var test = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                return test;
            }
            catch (Exception)
            {
                throw;
            }
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
                var Student_Id = Int32.Parse(Request.Form["StudentId"]);
                var grades = Request.Form["Grades"];                

                var values = new Learner_Values
                {
                    Behavioural_Statement = behaviouralId,                  
                    School_Year = _settingsService.GetSchoolYear(),
                    Student_Id = Student_Id,
                    Grade = grades
                };
                _studentManagementService.AddLearnerValues(values);

                return RedirectToAction("StudentValues", new { studentId = Student_Id , school_year = _settingsService.GetSchoolYear()});
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
                headSubjectAverage = Math.Round(sum / subjects.Count, MidpointRounding.AwayFromZero);
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
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Attendance added successfully!";
                return RedirectToAction("StudentAttendance", new { studentId = container.studentId });    
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                return RedirectToAction("StudentAttendance", new { studentId = container.studentId });
            }
        }
        public IActionResult StudentAttendance(int studentId)
        {
            try
            {
                var schoolYear = _settingsService.GetSchoolYear();
                var container = _studentManagementService.GetStudentAttendance(studentId,schoolYear);
                ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
                ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
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
