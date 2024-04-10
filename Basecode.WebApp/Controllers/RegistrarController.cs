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
using IronPdf;
using IronPdf.Extensions.Mvc.Core;
using iTextSharp.text.pdf;
using System.Globalization;
using Wkhtmltopdf.NetCore;
using AutoMapper;
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
        private readonly IRazorViewRenderer _viewRenderService;
        private readonly IMapper _mapper;   
        public RegistrarController(INewEnrolleeService newEnrolleeService,
            ITeacherService teacherService,
            ISubjectService subjectService,
            IClassManagementService classManagementService,
            IStudentManagementService studentManagementService,
            ISettingsService settingsService,
            IRazorViewRenderer viewRenderService,
            IMapper mapper) 
        { 
            _newEnrolleeService = newEnrolleeService;
            _teacherService = teacherService;
            _subjectService = subjectService;
            _classManagementService = classManagementService;
            _studentManagementService = studentManagementService;
            _settingsService = settingsService;     
            _viewRenderService = viewRenderService;
            _mapper = mapper;
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
        public async Task<IActionResult> StudentInfo(int student_Id, string school_year)
        {
            try
            {              
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
                var HasChildValue = Request.Form["HasChild"];
                bool HasChild = bool.Parse(HasChildValue);
                var subject = new Subject {
                    Subject_Name = subname,
                    Grade = grade,
                    HasChild = HasChild
                };                
                var id = _subjectService.AddSubject(subject);
                var headSubject = new HeadSubject
                {
                    Subect_Id = id
                };
                _subjectService.AddHeadSubejct(headSubject);

                if(HasChild)
                {
                    return RedirectToAction("AddChildSubjectForm", new { headId =id, grade =grade});
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
                return View(_subjectService.GetChildSubject(headId));
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
            return View(_settingsService.GetSettings());
        }
        [HttpPost]
        public IActionResult UpdateSettings(SettingsViewModel settings)
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
                    var document = new Rotativa.AspNetCore.ViewAsPdf("PDFView/ReportCardPDF", studentCard)
                    {
                        //FileName = studentCard.StudentDetails.Student.LastName+"_"+ studentCard.StudentDetails.Student.FirstName+"Report_Card.pdf",
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
    }
}
