using AutoMapper.Configuration.Conventions;
using Basecode.Data;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using Basecode.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata;
using System.Web;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
//using System.Web.Mvc;

namespace Basecode_WebApp.Controllers
{
    [Authorize(Roles = "Registrar")]
    public class RegistrarController : Controller
    {
        private bool succededOperation = true;
        private string ErrorMessage;
        private INewEnrolleeService _newEnrolleeService;
        private ITeacherService _teacherService;
        private ISubjectService _subjectService;
        private IClassManagementService _classManagementService;
        private IStudentManagementService _studentManagementService;
        private ISettingsService _settingsService;        
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IStudentService _studentService;
        public RegistrarController(INewEnrolleeService newEnrolleeService,
            ITeacherService teacherService,
            ISubjectService subjectService,
            IClassManagementService classManagementService,
            IStudentManagementService studentManagementService,
            ISettingsService settingsService,           
            IMapper mapper,
            IUsersService usersService,
            UserManager<IdentityUser> userManager,
            IStudentService studentService
            ) 
        { 
            _newEnrolleeService = newEnrolleeService;
            _teacherService = teacherService;
            _subjectService = subjectService;
            _classManagementService = classManagementService;
            _studentManagementService = studentManagementService;
            _settingsService = settingsService;                 
            _mapper = mapper;
            _usersService = usersService;
            _userManager = userManager;
            _studentService = studentService;

            //un enroll student if this date is at the end of the school year
            var datToday = DateTime.Now;
            var endSchoolDate = _settingsService.GetSettings().EndofClass;
            if (datToday.CompareTo(endSchoolDate) == 0)
                _studentService.UnEnrollStudents();
        }
        public async Task<IActionResult> Index(int gradeLevel, int quarter, int rank)
        {
            try
            {
                ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
                ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
                if (await _usersService.IsNewUser(_userManager.GetUserId(User)))
                {
                    return RedirectToAction("RegistrarProfileRegistration");
                }
                var dashboard = await _usersService.SetRegisrarDashBoard();
                dashboard.SchoolYear = _settingsService.GetSchoolYear();
                dashboard.StudentRanking = _studentManagementService.GetStudentRanking(gradeLevel, quarter, rank);
                return View(dashboard);
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult QueryDashboad()
        {
            try
            {
                var gradeLevel = Request.Form["grade"].ToString();
                var quarter = Int32.Parse(Request.Form["quarter"]);
                var rank = Int32.Parse(Request.Form["rank"]);

                return RedirectToAction("Index", new { gradeLevel = gradeLevel, quarter = quarter, rank = rank });
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public IActionResult RegistrarProfileRegistration()
        {
            ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
            ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
            return View();
        }
        public async Task<IActionResult> UserSetUpRegistrar(ProfileViewModel profile)
        {
            try
            {
                profile.AspUserId = _userManager.GetUserId(User);
                await _usersService.NewUserDetailsRegistration(profile);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Registrar's Profile created successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                return RedirectToAction("RegistrarProfileRegistration");
            }
        }
        public IActionResult StudentRecord()
        {
            try
            {
                ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
                ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
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
        public async Task<IActionResult> StudentInfo(int student_Id, string school_year)
        {
            try
            {
                ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
                ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
                if (school_year == null)
                {
                    var schoolYear = _settingsService.GetSettings().StartofClass.Value.Year.ToString() + "-" +
                _settingsService.GetSettings().EndofClass.Value.Year.ToString();
                    school_year = schoolYear;
                }
                
                return View(await _studentManagementService.GetStudentGrades(student_Id, school_year));
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Index");
            }         
        }
        public IActionResult RemoveStudent(int studentId)
        {
            try
            {
                _studentService.RemoveStudent(studentId);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Student removed successfully!";
                return RedirectToAction("StudentRecord");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                Console.WriteLine(ex);
                return RedirectToAction("StudentRecord");
            }

        }
        public async Task<IActionResult> EnrollAndPromoteStudent(int id)
        {
            try
            {
                var student = _studentService.GetStudent(id);
                student.status = "Enrolled";
                student.CurrGrade += 1;
                await _studentService.UpdateStudentAsync(student);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Successfully Enrolled and Promoted Student";
                return RedirectToAction("StudentInfo", new { student_Id = id});
            }
            catch(Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                Console.WriteLine(ex);
                return RedirectToAction("StudentInfo", new { student_Id = id });
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
            int id = Int32.Parse(Request.Form["id"]);
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
        [HttpPost]
        public IActionResult AdmitNewEnrollee()
        {
            try
            {
                var id = Int32.Parse(Request.Form["studentId"]);
                var lrn = Request.Form["lrn"];
                 _newEnrolleeService.AdmitNewEnrollee(id, lrn);
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
        public async Task<IActionResult>ManageTeachersList()
        {
            try
            {
                ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
                ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
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
        public async Task<IActionResult> RemoveTeacher(string aspid)
        {
            try
            {
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Teacher Removed Successfully!";
                await _teacherService.RemoveTeacher(aspid);
                return RedirectToAction("ManageTeachersList");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                Console.WriteLine(ex);
                return RedirectToAction("ManageTeachersList");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ManageTeachersListDataTable()
        {
            try
            {
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
                var customerData = await _teacherService.GetTeacherinitView();
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
                    else if(sortColumn.Equals("lastname"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.lastname) :
                            asEnumcustomerData.OrderByDescending(p => p.lastname);
                    }
                    else if(sortColumn.Equals("gender"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.gender) :
                            asEnumcustomerData.OrderByDescending(p => p.gender);
                    }
                    else if (sortColumn.Equals("email"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.email) :
                            asEnumcustomerData.OrderByDescending(p => p.email);
                    }

                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    asEnumcustomerData = customerData.Where(m => m.firstname.ToLower().Contains(searchValue.ToString().ToLower())
                     || m.middlename.ToLower().Contains(searchValue.ToString().ToLower()) || m.lastname.ToLower().Contains(searchValue.ToString().ToLower()) ||
                     m.gender.ToLower().Contains(searchValue.ToString().ToLower()) || m.email.ToLower().Contains(searchValue.ToString().ToLower()));
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
        public IActionResult ManageSubjects()
        {
            ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
            ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
            return View();
        }
        [HttpPost]
        public IActionResult ManageSubjectsDataTable()
        {
            try
            {
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
                var customerData = _subjectService.GetSubjectsForDataTable().AsEnumerable();

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumn.Equals("subjectname"))
                    {
                        customerData = sortColumnDirection.ToLower() == "asc" ? customerData.OrderBy(p => p.subjectname) :
                            customerData.OrderByDescending(p => p.subjectname);
                    }
                    else if (sortColumn.Equals("grade"))
                    {
                        customerData = sortColumnDirection.ToLower() == "asc" ? customerData.OrderBy(p => p.grade):
                            customerData.OrderByDescending(p => p.grade);
                    }
                    
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.subjectname.ToLower().Contains(searchValue.ToString().ToLower())
                     || m.grade.ToString().Equals(searchValue));
                }

                //total number of rows count   
                recordsTotal = customerData.Count();
                //Paging   
                var data = customerData.Skip(skip).Take(pageSize);
                //Returning Json Data  
                var test = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                return test;
            }
            catch (Exception)
            {
                throw;
            }
        }        
        public IActionResult AddSubject(SubjectViewModel subjectview)
        {
            try
            {                
                var subject = new Subject {
                    Subject_Name = subjectview.subjectname,
                    Grade = subjectview.grade,
                    HasChild = subjectview.haschild
                };                
                var id = _subjectService.AddSubject(subject);
                var headSubject = new HeadSubject
                {
                    Subect_Id = id
                };
                _subjectService.AddHeadSubejct(headSubject);

                if(subjectview.haschild)
                {
                    return RedirectToAction("AddChildSubjectForm", new { headId =id, grade = subjectview.grade });
                }
                _subjectService.SaveDbChanges();
                return RedirectToAction("ManageSubjects");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public IActionResult AddChildSubjectForm(int headId, int grade)
        {
            try
            {
                var form = new ChildSubjectForm
                {
                    HeadId = headId,
                    Grade = grade
                };
                return View(form);
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult AddChildSubject()
        {
            try
            {
                var headId = Int32.Parse(Request.Form["headId"]);
                var grade = Int32.Parse(Request.Form["grade"]);
                var input = Request.Form["inputSubname"];                
                for(int x = 0; x< input.Count; x++)
                {
                    var subname = input[x];                   
                    var subject = new Subject
                    {
                        Subject_Name = subname,
                        Grade = grade,
                        HasChild = false
                    };
                    var id = _subjectService.AddSubject(subject);
                    var childsubject = new ChildSubject
                    {
                        HeadSubjectId = headId,
                        Subject_Id = id,                        
                    };
                    _subjectService.AddChildSubject(childsubject);                   
                }
                return RedirectToAction("ManageSubjects");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public IActionResult ViewChildSubject(int headId)
        {
            try
            {
                ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
                ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
                return View(_subjectService.GetChildSubject(headId));
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        } 
        public IActionResult AddSingleChildSubject(ChildSubjectContainer childSubject)
        {

            try
            {
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Subject created successfully!";
                _subjectService.AddChildSubject(childSubject);
                return RedirectToAction("ViewChildSubject", new { headId = childSubject.HeadId });
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;                
                Console.WriteLine(ex);
                return RedirectToAction("ViewChildSubject", new { headId = childSubject.HeadId });
            }
        }
        public IActionResult UpdateChildSubject(ChildSubjectContainer childSubject)
        {
            try
            {
                var sub = new ChildSubjectView()
                {
                    Id = childSubject.ChildSubId,
                    Name = childSubject.Name
                };
                _subjectService.UpdateChildSubject(sub);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Subject updated!";
                return RedirectToAction("ViewChildSubject", new { headId = childSubject.HeadId});
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("ViewChildSubject", new { headId = childSubject.HeadId });
            }
        }
        public IActionResult RemoveSubject(int id, int subjectId,int headid)
        {
            try
            {
                _subjectService.RemoveChildSubject(id);
                _subjectService.RemoveSubject(subjectId);                
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Subject Removed!";
                return RedirectToAction("ViewChildSubject", new { headId = headid });
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("ViewChildSubject", new { headId = headid });
            }
        }       
        public IActionResult RemoveSubjectHead(int id)
        {
            try
            {
                _subjectService.RemoveSubject(id);
                var subject = _subjectService.GetSubject(id);
                //if(subject.HasChild)

                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Subject Removed!";
                return RedirectToAction("ManageSubjects");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("ManageSubjects");
            }
        }
        public async Task<IActionResult> ManageClass()
        {
            ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
            ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
            var classes = await _classManagementService.GetAllClass();
            return View(classes);
        }
        public IActionResult RemoveClass(int classId)
        {
            try
            {
                _classManagementService.RemoveClass(classId);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Class Removed Successfully!";
                return RedirectToAction("ManageClass");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("ClassDetails", new { classId = classId });
            }
        }
        public IActionResult UpdateClass(ClassViewModel classview)
        {
            try
            {
                _classManagementService.UpdateClass(classview);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Class updated!";
                return RedirectToAction("ClassDetails", new { classId = classview.Id });
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("ClassDetails", new { classId = classview.Id });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ManageClassDataTable()
        {
            //var classes = await _classManagementService.GetAllClass();
            //return View(classes);
            try
            {
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
                var customerData = await _classManagementService.GetAllClass();

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumn.Equals("classname"))
                    {
                        customerData.classes = sortColumnDirection.ToLower() == "asc" ? customerData.classes.OrderBy(p => p.classname).ToList() :
                            customerData.classes.OrderByDescending(p => p.classname).ToList();
                    }
                    else if (sortColumn.Equals("advisername"))
                    {
                        customerData.classes = sortColumnDirection.ToLower() == "asc" ? customerData.classes.OrderBy(p => p.advisername).ToList():
                            customerData.classes.OrderByDescending(p => p.advisername).ToList();
                    }
                    else if (sortColumn.Equals("grade"))
                    {
                        customerData.classes = sortColumnDirection.ToLower() == "asc" ? customerData.classes.OrderBy(p => p.grade).ToList() :
                           customerData.classes.OrderByDescending(p => p.grade).ToList();
                    }
                    else if (sortColumn.Equals("classsize"))
                    {
                        customerData.classes = sortColumnDirection.ToLower() == "asc" ? customerData.classes.OrderBy(p => p.classsize).ToList() :
                           customerData.classes.OrderByDescending(p => p.grade).ToList();
                    }
                    else if (sortColumn.Equals("schoolyear"))
                    {
                        customerData.classes = sortColumnDirection.ToLower() == "asc" ? customerData.classes.OrderBy(p => p.schoolyear).ToList():
                          customerData.classes.OrderByDescending(p => p.schoolyear).ToList();
                    }

                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData.classes = customerData.classes.Where(m => m.classname.ToLower().Contains(searchValue.ToString().ToLower())
                     || m.grade.ToString().Equals(searchValue) || m.grade.ToString().Equals(searchValue)
                     || m.schoolyear.ToLower().Contains(searchValue.ToString().ToLower()) || m.advisername.ToLower().Contains(searchValue.ToString().ToLower())).ToList();
                }

                //total number of rows count   
                recordsTotal = customerData.classes.Count();
                //Paging   
                var data = customerData.classes.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                var test = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                return test;
            }
            catch (Exception)
            {
                throw;
            }
        }       
        public IActionResult AddClass(Class inputclass)
        {
            try
            {
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Class created successfully!";
                _classManagementService.AddClass(inputclass);
                return RedirectToAction("ManageClass");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("ManageClass");
            }
        }
        public async Task<IActionResult> ClassDetails(int classId)
        {
            try
            {
                ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
                ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
                var classdetails = await _classManagementService.GetClassViewModelById(classId);
                return View(classdetails);
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
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
        public IActionResult AddClassStudents(ClassViewModel model)
        {
            try
            {                            
                var students = new List<ClassStudents>();
                foreach(var student in model.SelectedStudents)
                {
                    var cs = new ClassStudents
                    {
                        Class_Id = model.Id,
                        Student_Id = student
                    };
                    students.Add(cs);
                }
               // _classManagementService.AddClassStudent(classStudent);
                _classManagementService.AddClassStudents(students);
                return RedirectToAction("ClassDetails", new { classId = model.Id });
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
        public IActionResult Settings()
        {
            ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
            ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
            return View(_settingsService.GetSettings());
        }
        
        public IActionResult UpdateSettings(SettingsViewModel settings)
        {
            try
            {
                _settingsService.UpdateSettings(settings);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Successfully updated the settings";
                return RedirectToAction("Settings");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                Console.WriteLine(ex);
                return RedirectToAction("Settings");
            }
        }
        public IActionResult ManageLearnerValues()
        {
            try
            {
                var learnerValues = new Learner_Values_ViewModel();
                learnerValues.CoreValues = _studentManagementService.GetAllCoreValues();
                return View(learnerValues);
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult AddCoreValue()
        {
            try
            {
                var core_value = Request.Form["Core_Value"];
                var core_value_object = new Core_Values
                {
                    core_Values = core_value
                };
                _studentManagementService.AddCoreValues(core_value_object);
                return RedirectToAction("ManageLearnerValues");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public IActionResult AddBehavioralStatement(Learner_Values_ViewModel model)
        {
            try
            {
                var statement = new Behavioural_Statement();
                statement.Core_Values = model.Core_Values;
                statement.Statements = model.Behavioural_Statement;
                _studentManagementService.AddBehavioralStatement(statement);
                return RedirectToAction("ManageLearnerValues");   
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public IActionResult ViewLearnersValues()
        {
            try
            {
                var test = _studentManagementService.GetLearnersValues();
                return View(_studentManagementService.GetLearnersValues());
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public IActionResult UpdateCoreValues(Core_Values values)
        {
            try
            {
                _studentManagementService.UpdateCoreValues(values);
                return RedirectToAction("ViewLearnersValues");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public IActionResult UpdateBeheviouralStatement(Learner_Values_ViewModel model)
        {
            try
            {
                var statement = new Behavioural_Statement()
                {
                    Id = model.Id,
                    Core_Values = model.Core_Values,
                    Statements = model.Behavioural_Statement
                };
                _studentManagementService.UpdateBehavioralStatement(statement);
                return RedirectToAction("ViewLearnersValues");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public IActionResult DeleteCoreValues(Core_Values values)
        {
            try
            {
                _studentManagementService.DeleteCoreValues(values);
                return RedirectToAction("ViewLearnersValues");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public IActionResult UpdateBehaviouralStatementView(int id)
        {
            try
            {
                var statement = _studentManagementService.GetBehaviouralStatementById(id);
                var values = new Learner_Values_ViewModel()
                {
                    Id = id,
                    Behavioural_Statement = statement.Statements,
                    Core_Values = statement.Core_Values,
                    CoreValues = _studentManagementService.GetAllCoreValues()
                };
                return View(values);
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public IActionResult RemoveCoreValues(int id)
        {
            try
            {
                var corevalues = _studentManagementService.GetCoreValuesById(id);
                _studentManagementService.DeleteCoreValues(corevalues);
                return RedirectToAction("ViewLearnersValues");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult UpdateCoreValues()
        {
            try
            {
                var corevalues = Request.Form["corevalues"];
                var corevaluesId = Request.Form["id"];
                var cv = new Core_Values()
                {
                    core_Values = corevalues,
                    Id = Int32.Parse(corevaluesId),
                };
                _studentManagementService.UpdateCoreValues(cv);

                return RedirectToAction("ViewLearnersValues");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public IActionResult TestPdf()
        {            
            return View();
        }
        public async Task<IActionResult> GeneratePdf(int StudentId, string SchoolYear, int documentType)
        {
            try
            {                
                ReportCardContents studentCard = new ReportCardContents();
                studentCard.Settings = _settingsService.GetSettings();
                if(documentType == 0)
                {
                    studentCard.StudentDetails = await _studentManagementService.GetStudentGrades(StudentId, SchoolYear);
                    if(studentCard.StudentDetails.studentClass == null)
                    {
                        Constants.ViewDataErrorHandling.Success = 0;
                        Constants.ViewDataErrorHandling.ErrorMessage = "The student is currently not added to any class.";
                        return RedirectToAction("StudentInfo", new { student_Id = StudentId });
                    }
                    var document = new Rotativa.AspNetCore.ViewAsPdf("PDFView/ReportCardPDF", studentCard)
                    {
                        //FileName = studentCard.StudentDetails.Student.lastname+"_"+ studentCard.StudentDetails.Student.firstname+"Report_Card.pdf",
                        PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                        PageMargins = { Left = 10, Bottom = 10, Right = 10, Top = 10 }
                    };
                    return document;
                }
                else
                {
                    var form137 =await _studentManagementService.GetStudentForm137(StudentId);
                    return new Rotativa.AspNetCore.ViewAsPdf("PDFView/Form137PDF", form137);
                }
               
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public IActionResult UpdateSchoolLogo(SettingsViewModel settingsViewModel)
        {
            try
            {
                var settings = _settingsService.GetSettingsById(settingsViewModel.Id);
                var newSettingViewModel = _mapper.Map<SettingsViewModel>(settings);                
                newSettingViewModel.SchoolLogoRecieve = settingsViewModel.SchoolLogoRecieve;
                _settingsService.UpdateSettings(newSettingViewModel);
                return RedirectToAction("Settings");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public IActionResult TeacherRegistration()
        {
            try
            {
                ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
                ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }

        }
        public async Task<IActionResult> RegisterTeacher(UsersRegistration account)
        {
            try
            {
                await _teacherService.AddTeacherUserAccount(account);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Teacher's account created successfully!";
                return RedirectToAction("ManageTeachersList");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                Console.WriteLine(ex);
                return RedirectToAction("TeacherRegistration");
            }
        }
        public IActionResult RejectTeacherRegistration(int id)
        {
            try
            {
                _teacherService.RejectTeacherRegistration(id);
                return RedirectToAction("TeacherRegistration");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> AcceptTeacherRegistration(int id)
        {
            try
            {
                await _teacherService.ApproveTeacherRegistration(id);
                return RedirectToAction("TeacherRegistration");
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public  async Task<IActionResult> Profile()
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
        [HttpPost]
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
        public IActionResult AddNewEnrollee()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {                
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        public IActionResult AddNewStudent(RegisterStudent newStudent)
        {
            try
            {               
                _studentService.AddStudent(newStudent);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Successfully added new student!";
                return RedirectToAction("StudentRecord");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                Console.WriteLine(ex);
                return RedirectToAction("StudentRecord");
            }
        }
        public async Task<IActionResult> UpdateStudentDetailsPage(int studentId)
        {
            try
            {
                var studentContainer = await _studentManagementService.GetStudentDetails(studentId);
                return View(studentContainer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("StudentRecord");
            }
        }
        public async Task<IActionResult> UpdateStudentDetails(StudentDetailsContainer studentDetails)
        {
            try
            {
                await _studentManagementService.UpdateStudentDetails(studentDetails);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Successfully updated the student!";
                return RedirectToAction("StudentInfo", new {  student_Id = studentDetails.Student.Student_ID });
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                Console.WriteLine(ex);
                return RedirectToAction("StudentRecord");
            }
        }
        [HttpPost]
        public IActionResult StudentRecordTest()
        {
            try
            {
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
                var customerData = _studentManagementService.GetStudentPreviewInformation();

                //Sorting
                if(!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if(sortColumn.Equals("fullname"))
                    {
                        customerData = sortColumnDirection.ToLower() == "asc"? customerData.OrderBy(p => p.fullname): 
                            customerData.OrderByDescending(p => p.fullname);
                    }
                    else if(sortColumn.Equals("status"))
                    {
                        customerData = sortColumnDirection.ToLower() == "asc" ? customerData.OrderBy(p => p.status) :
                            customerData.OrderByDescending(p => p.status);
                    }
                    else if(sortColumn.Equals("age"))
                    {
                        customerData = sortColumnDirection.ToLower() == "asc" ? customerData.OrderBy(p => p.age) :
                           customerData.OrderByDescending(p => p.age);
                    }
                    else if(sortColumn.Equals("lrn"))
                    {
                        customerData = sortColumnDirection.ToLower() == "asc" ? customerData.OrderBy(p => p.lrn) :
                           customerData.OrderByDescending(p => p.lrn);
                    }
                    else if(sortColumn.Equals("grade"))
                    {
                        customerData = sortColumnDirection.ToLower() == "asc" ? customerData.OrderBy(p => p.grade) :
                          customerData.OrderByDescending(p => p.grade);
                    }

                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.fullname.ToLower().Contains(searchValue.ToString().ToLower())                   
                     || m.grade.ToString().Equals(searchValue) || m.age.ToString().Equals(searchValue) 
                     || m.status.ToLower().Contains(searchValue.ToString().ToLower()) || m.lrn.ToLower().Contains(searchValue.ToString().ToLower()));
                }

                //total number of rows count   
                recordsTotal = customerData.Count();
                //Paging   
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                var test = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                return test;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IActionResult TestDataTable()
        {
            return View();
        }
    }
}
