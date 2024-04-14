using Basecode.Data.Interfaces;
using Basecode.Data.ViewModels;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basecode.Data;
using Basecode.Data.Models;
using Basecode.Services.Interfaces;
using System.Globalization;
using System.Drawing;
using System.Xml;
namespace Basecode.Services.Services
{
    public class StudentManagementService:IStudentManagementService
    {
        private readonly IStudentManagementRepository _studentManagementRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ISettingsRepository _settingsRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IClassManagementRepository _classManagementRepository;
        private readonly IParentRepository _parentRepository;
        public StudentManagementService(IStudentManagementRepository studentManagementRepository,
            IStudentRepository studentRepository,
            ISettingsRepository settings,
            ISubjectRepository subjectRepository,
            IClassManagementRepository classManagementRepository,
            IParentRepository parentRepository) 
        {
            _studentManagementRepository = studentManagementRepository;
            _studentRepository = studentRepository;
            _settingsRepository = settings;
            _subjectRepository = subjectRepository;
            _classManagementRepository = classManagementRepository;
            _parentRepository = parentRepository;
        }
        public GradesDetail GetStudentGradeBySubject(int student_Id, int subject_Id)
        {
            try
            {
                return _studentManagementRepository.GetStudentGradeBySubject(student_Id, subject_Id);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void SubmitGrade(int student_id, int Subject_Id, int grade, int Quarter)
        {
            try
            {
                var student = _studentRepository.GetStudent(student_id);
                var grades = new Grades
                {
                    Student_Id = student_id,
                    Subject_Id = Subject_Id,
                    Grade = grade,
                    Quarter =   Quarter,
                    Grade_Level = student.CurrGrade,
                    School_Year =_settingsRepository.GetSchoolYear()
                };
                _studentManagementRepository.SubmitGrade(grades);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void EditGrade(int Grade_Id, int student_id, int Subject_Id, int grade, int Quarter)
        {
            try
            {
                var student = _studentRepository.GetStudent(student_id);
                var grades = new Grades
                {
                    Grade_Id = Grade_Id,
                    Student_Id = student_id,
                    Subject_Id = Subject_Id,
                    Grade = grade,
                    Quarter = Quarter,
                    Grade_Level = student.CurrGrade,
                    School_Year = _settingsRepository.GetSchoolYear()
                };
                _studentManagementRepository.EditGrade(grades);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public ChildSubjectGrades GetChildSubjectGrades(int headId, int studentId)
        {
            try
            {
                var childSubjectGrade = new ChildSubjectGrades();
                childSubjectGrade.ChildSubjects = _subjectRepository.GetChildSubject(headId);
                childSubjectGrade.GradesContainer = new List<GradesDetail>();
                childSubjectGrade.HeadId = headId;
                childSubjectGrade.StudentId = studentId;
                foreach(var subject in childSubjectGrade.ChildSubjects)
                {
                    childSubjectGrade.GradesContainer.Add(_studentManagementRepository.GetStudentGradeBySubject(studentId, subject.Id));
                }
                return childSubjectGrade;
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public List<Core_Values> GetAllCoreValues()
        {
            try
            {
                return _studentManagementRepository.GetAllCoreValues();
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public async Task<StudentDetailsWithGrade> GetStudentGrades(int student_Id, string school_year)
        {
            try
            {
                var headSubjects = _subjectRepository.GetAllHeadSubject();
                var student = new StudentDetailsWithGrade();
                student.School_Years = _studentManagementRepository.GetSchoolYears(student_Id);
                student.Student = _studentManagementRepository.GetStudent(student_Id);
                student.grades = _studentManagementRepository.GetStudentGrades(student_Id,school_year);
                student.valuesGrades = _studentManagementRepository.GetValuesGrades(student_Id,school_year);
                student.learnersValues = _studentManagementRepository.GetLearnersValues();
                student.StudentAttendance = this.GetStudentAttendance(student_Id,school_year);
                student.Subjects = _subjectRepository.GetAllSubjects(student_Id, school_year);
                student.studentClass = await _classManagementRepository.GetClassWhereStudentBelong(student_Id,school_year);
                var unionHeadSubject = from h in headSubjects
                                       join s in student.Subjects
                                        on h.Subect_Id equals s.Subject_Id
                                       select new
                                       {
                                           
                                       };

                student.TotalHeadSubjectCount = unionHeadSubject.Count();
                student.SchoolYear = school_year;
                student.Parent = _parentRepository.GetParentDetailById(student_Id);
                return student;
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public async Task<Form137Container> GetStudentForm137(int studentId)
        {
            try
            {


                var schoolYears = _studentManagementRepository.GetSchoolYears(studentId);
                var form137Container = new Form137Container();
                form137Container.Student = _studentManagementRepository.GetStudent(studentId);
                form137Container.Settings = _settingsRepository.GetSettings(); 
                form137Container.StudentForm137 = new List<Form137ViewModel>();
                
                foreach (var schoolYear in schoolYears)
                {
                    var form137 = new Form137ViewModel();                    
                    form137.SchoolYear = schoolYear;
                    form137.GradeLevel = await _classManagementRepository.GetStudentYearLevel(studentId, schoolYear);
                    form137.grades = _studentManagementRepository.GetStudentGrades(studentId, schoolYear);
                    form137.Subjects = _subjectRepository.GetAllSubjects(studentId, schoolYear);

                    var headSubjects = _subjectRepository.GetAllHeadSubject();
                    var unionHeadSubject = from h in headSubjects
                                           join s in form137.Subjects
                                            on h.Subect_Id equals s.Subject_Id
                                           select new
                                           {

                                           };
                    form137.TotalHeadSubjectCount = unionHeadSubject.Count();
                    form137Container.StudentForm137.Add(form137);
                }
                return form137Container;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public List<StudentViewModel> GetAllStudents()
        {
            try
            {
                return _studentManagementRepository.GetAllStudents();
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void AddCoreValues(Core_Values values)
        {
            try
            {
                _studentManagementRepository.AddCoreValues(values);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void AddBehavioralStatement(Behavioural_Statement statement)
        {
            try
            {
                _studentManagementRepository.AddBehaviouralStatement(statement);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public List<Learners_Values_Report> GetLearnersValues()
        {
            try
            {
                return _studentManagementRepository.GetLearnersValues();
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void UpdateCoreValues(Core_Values values)
        {
            try
            {
                _studentManagementRepository.UpdateCoreValues(values);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void UpdateBehavioralStatement(Behavioural_Statement statement)
        {
            try
            {
                _studentManagementRepository.UpdateBehavioralStatement(statement);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void DeleteCoreValues(Core_Values values)
        {
            try
            {
                _studentManagementRepository.DeleteCore_Values(values);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public Behavioural_Statement GetBehaviouralStatementById(int Id)
        {
            try
            {
                return _studentManagementRepository.GetBehaviouralStatementById(Id);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public Core_Values GetCoreValuesById(int Id)
        {
            try
            {
                return _studentManagementRepository.GetCoreValuesById(Id);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public ValuesWithGradesContainer GetValuesWithGrades(int StudentId, string schoolyear)
        {
            try
            {
                var valueswithgrades = new ValuesWithGradesContainer();
                valueswithgrades.Student_Id = StudentId;
                valueswithgrades.School_Year = schoolyear;
                valueswithgrades.Grades = _studentManagementRepository.GetValuesGrades(StudentId, schoolyear);
                valueswithgrades.Values = _studentManagementRepository.GetLearnersValues();
                valueswithgrades.Scool_Years = _studentManagementRepository.GetValuesSchoolyear(StudentId);
                return valueswithgrades;
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void AddLearnerValues(Learner_Values values)
        {
            try
            {
                _studentManagementRepository.AddLearnerValues(values);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void UpdateLearnerValues(int id, string grade)
        {
            try
            {
                var valuesgrade = _studentManagementRepository.GetLearnerValuesById(id);
                valuesgrade.Grade = grade;
                _studentManagementRepository.UpdateLearnerValues(valuesgrade);
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void AddStudentAttendance(int studentId,int Days_of_School, int Days_of_Present, int Time_of_Tardy, string month)
        {
            try
            {                
                var schoolYear = _settingsRepository.GetSchoolYear();               
                //var thisMonth = DateTime.Now.Date;
                var attendance = new Attendance();
                var numMonth = DateTime.ParseExact(Helper.GetFullMonthName(month), "MMMM", CultureInfo.CurrentCulture, DateTimeStyles.None).Month;
                if (!_studentManagementRepository.isDateExisting(numMonth, schoolYear))
                {
                    attendance.Studentid = studentId;
                    attendance.Days_of_Schoool = Days_of_School;
                    attendance.Days_of_Present = Days_of_Present;
                    attendance.School_Year = schoolYear;
                    attendance.Month = numMonth;
                    attendance.Time_of_Tardy = Time_of_Tardy;
                    _studentManagementRepository.AddAttendance(attendance);
                }
                else
                    throw new Exception("The attendance for this month is already Existing");
            }
            catch(Exception ex)
            {
                Console.Write(ex);
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public AttendanceContainer GetStudentAttendance(int studentId, string schoolYear)
        {
            try
            {
                var listAttendance = _studentManagementRepository.GetStudentAtendance(studentId, schoolYear);
                List<string> Months = new List<string>();
                List<int> MonthsInt = new List<int>();
                List<Attendance> containerAttendance = new List<Attendance>();
                var attendanceContainer = new AttendanceContainer();

                var StartSchoolDate = _settingsRepository.GetSettings().StartofClass;
                var EndSchoolDate = _settingsRepository.GetSettings().EndofClass;
                var currentDate = StartSchoolDate;
                DateTimeFormatInfo dtfi = new DateTimeFormatInfo();

                while (currentDate<= EndSchoolDate)
                {                    
                    Months.Add(dtfi.GetAbbreviatedMonthName(currentDate.Value.Month));
                    var attendance = listAttendance.FirstOrDefault(p => p.Month == currentDate.Value.Month);
                    
                    containerAttendance.Add(attendance);

                    currentDate = currentDate.Value.AddMonths(1);
                }
                attendanceContainer.studentId = studentId;
                attendanceContainer.Months = Months;
                attendanceContainer.Attendances = containerAttendance;

                return attendanceContainer;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public void UpdateAttendance(int id,int studentId, int Days_of_School, int Days_of_Present, int Time_of_Tardy, string month)
        {
            try
            {
                var schoolYear = _settingsRepository.GetSchoolYear();
                var numMonth = DateTime.ParseExact(Helper.GetFullMonthName(month), "MMMM", CultureInfo.CurrentCulture, DateTimeStyles.None).Month;
                var attendance = new Attendance();

                if (_studentManagementRepository.isDateExisting(numMonth, schoolYear))
                    throw new Exception("Month already existing");

                attendance.Id = id;
                attendance.Studentid = studentId;
                attendance.Days_of_Schoool = Days_of_School;
                attendance.Days_of_Present = Days_of_Present;
                attendance.School_Year = schoolYear;
                attendance.Month = numMonth;
                attendance.Time_of_Tardy = Time_of_Tardy;

                _studentManagementRepository.UpdateAttendance(attendance);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Data.Constants.Exception.DB);
            }
        }

    }
}
