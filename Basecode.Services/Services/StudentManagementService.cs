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
        private readonly IUsersRepository _usersRepository;
        private readonly IRTPRepository _rtpRepository;
        private readonly ISettingsService _settingsService;
        public StudentManagementService(IStudentManagementRepository studentManagementRepository,
            IStudentRepository studentRepository,
            ISettingsRepository settings,
            ISubjectRepository subjectRepository,
            IClassManagementRepository classManagementRepository,
            IParentRepository parentRepository,
            IUsersRepository usersRepository,
            IRTPRepository rtpRepository,
            ISettingsService settingsService) 
        {
            _studentManagementRepository = studentManagementRepository;
            _studentRepository = studentRepository;
            _settingsRepository = settings;
            _subjectRepository = subjectRepository;
            _classManagementRepository = classManagementRepository;
            _parentRepository = parentRepository;
            _usersRepository = usersRepository;
            _rtpRepository = rtpRepository;
            _settingsService = settingsService;
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
                var headsub = _subjectRepository.GetSubjectById(headId);
                childSubjectGrade.HeadSubjectGrade = _studentManagementRepository.GetStudentGradeBySubject(studentId, headId);
                childSubjectGrade.ChildSubjects = _subjectRepository.GetChildSubjects(headId);
                childSubjectGrade.GradesContainer = new List<GradesDetail>();
                childSubjectGrade.HeadSubjectName = headsub.Subject_Name;
                childSubjectGrade.HeadId = headId;
                childSubjectGrade.StudentId = studentId;               
                
                foreach (var subject in childSubjectGrade.ChildSubjects)
                {
                    childSubjectGrade.GradesContainer.Add(_studentManagementRepository.GetStudentGradeBySubject(studentId, subject.subjectId));
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
                student.Parent = await _parentRepository.GetParentDetailById(student_Id);
                return student;
            }
            catch
            {
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public List<StudentQuarterlyAverage> GetStudentRanking(int gradeLevel, int quarter, int rank)
        {
            try
            {
                double sum;
                int avg = 0;
                var students = _studentManagementRepository.GetAllStudentPreview().Where(p => p.grade == gradeLevel);
                var RankOfStudents = new List<StudentQuarterlyAverage>();

                foreach (var student in students)
                {
                    sum = 0.0;
                    var studentgrade = _studentManagementRepository.GetStudentGrades(student.studentid, _settingsRepository.GetSchoolYear());
                    var subjects = _subjectRepository.GetAllSubjectTakenByStudent(student.studentid, _settingsRepository.GetSchoolYear());
                    foreach (var subject in subjects)
                    {
                        var filteredSubject = studentgrade.FirstOrDefault(p => p.SubjectId == subject.Subject_Id);
                        if(filteredSubject != null)
                        {
                            var selectedGrade = filteredSubject.Grades.FirstOrDefault(p => p.Quarter == quarter);
                            if (selectedGrade == null)
                            {
                                throw new Exception("There are students that do not have grade on this particular quarter yet.");
                            }
                            sum += selectedGrade.Grade;
                        }
                        else
                        {
                            throw new Exception("There are students that do not have grades on some subjects.");
                        }
                    }
                    avg = (int)Math.Round(sum / subjects.Count(), MidpointRounding.AwayFromZero);

                    if ((rank == 1) && (avg >= _settingsService.GetWithHighestHonor() && avg <= 100))
                    {
                        var studentRank = new StudentQuarterlyAverage
                        {
                            studentId = student.studentid,
                            fullname = student.fullname,
                            Average = avg
                        };
                        RankOfStudents.Add(studentRank);
                    }
                    else if ((rank == 2) && (avg >= _settingsService.GetWithHighHonor() && avg < _settingsService.GetWithHighestHonor()))
                    {
                        var studentRank = new StudentQuarterlyAverage
                        {
                            studentId = student.studentid,
                            fullname = student.fullname,
                            Average = avg
                        };
                        RankOfStudents.Add(studentRank);
                    }
                    else if ((rank == 3) && (avg >= _settingsService.GetWithHonor() && avg < _settingsService.GetWithHighHonor()))
                    {
                        var studentRank = new StudentQuarterlyAverage
                        {
                            studentId = student.studentid,
                            fullname = student.fullname,
                            Average = avg
                        };
                        RankOfStudents.Add(studentRank);
                    }
                }
                return RankOfStudents;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }
        public async Task<StudentDetailsContainer> GetStudentDetails(int studentId)
        {
            try
            {
                var studentDetails = new StudentDetailsContainer();
                studentDetails.Student = _studentManagementRepository.GetStudent(studentId);
                studentDetails.Parent = await _parentRepository.GetParentDetailById(studentId);
                return studentDetails;
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
                int maxQuarter = _studentManagementRepository.GetBehavioralMaxQuarter(values.Student_Id,values.Behavioural_Statement,values.School_Year);
                values.Quarter = maxQuarter + 1;
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
                if (!_studentManagementRepository.isDateExisting(numMonth, schoolYear, studentId))
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
                throw new Exception(ex.Message);
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

                if (_studentManagementRepository.isDateExisting(numMonth, schoolYear, studentId))
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
        public async Task UpdateStudentDetails(StudentDetailsContainer studentDetails)
        {
            try
            {
                var student = _studentRepository.GetStudent(studentDetails.Student.Student_ID);
                var studentUserPortal = _usersRepository.GetUserById(student.UID);
                var parent = _parentRepository.GetParentById(student.ParentId);
                var parentUserPortal = _usersRepository.GetUserById(parent.UID);
                var parentRTP = _rtpRepository.GetRTPCommonsByUID(parent.UID);

                studentUserPortal.FirstName = studentDetails.Student.FirstName;
                studentUserPortal.MiddleName = studentDetails.Student.MiddleName;
                studentUserPortal.LastName = studentDetails.Student.LastName;
                studentUserPortal.Birthday = studentDetails.Student.Birthday;
                studentUserPortal.sex = studentDetails.Student.sex;
                studentUserPortal.ProfilePic = studentDetails.Student.profilePicture;

                student.LRN = studentDetails.Student.lrn;
                student.status = studentDetails.Student.Status;
                student.CurrGrade = studentDetails.Student.Grade;

                await _usersRepository.UpdateUserPortal(studentUserPortal);
                await _studentRepository.UpdateStudentAsync(student);

                parent.Email = studentDetails.Parent.Email;
                parentRTP.Address = studentDetails.Parent.Address;
                parentRTP.PhoneNumber = studentDetails.Parent.PhoneNumber;

                parentUserPortal.FirstName = studentDetails.Parent.ParentFirstName;
                parentUserPortal.MiddleName = studentDetails.Parent.ParentMiddleName;
                parentUserPortal.LastName = studentDetails.Parent.ParentLastName;
                parentUserPortal.Birthday = studentDetails.Parent.ParentBirthday;
                parentUserPortal.sex = studentDetails.Parent.Parentsex;

                await _usersRepository.UpdateUserPortal(parentUserPortal);
                await _parentRepository.UpdateParentAsyn(parent);
                await _rtpRepository.UpdateCommonsAsync(parentRTP);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public IEnumerable<StudentPreviewInformation> GetStudentPreviewInformation()
        {
            try
            {
                return _studentManagementRepository.GetAllStudentPreview();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        public List<ClassStudentViewModel> GetStudentWithNoGradePerQuarter(int classid, int subjectid, int quarter)
        {
            try
            {
                var students = _classManagementRepository.GetClassStudents(classid);
                var listOfNoGrades = new List<ClassStudentViewModel>();

                if(students.Count > 0 || students !=null)
                {
                    foreach (var student in students)
                    {
                        var studentgrade = _studentManagementRepository.GetStudentGradeBySubject(student.studentid, subjectid);
                        if (studentgrade.Grades.Where(p => p.Quarter == quarter).Count() == 0)
                            listOfNoGrades.Add(student);
                    }
                }                
                return listOfNoGrades;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(Data.Constants.Exception.DB);
            }
        }
        
    }
}
