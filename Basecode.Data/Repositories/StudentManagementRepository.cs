using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Repositories
{
    public class StudentManagementRepository : BaseRepository, IStudentManagementRepository
    {
        private readonly BasecodeContext _context;
        public StudentManagementRepository(IUnitOfWork unitOfWork,
            BasecodeContext context) : base(unitOfWork)
        {
            _context = context;
        }      
        public void SubmitGrade(Grades grade)
        {
            try
            {
                _context.Grades.Add(grade);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public void AddStudentAdviser(StudentAdviser studentAdviser)
        {
            try
            {
                _context.StudentAdviser.Add(studentAdviser);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public void AddCoreValues(Core_Values core_Values)
        {
            try
            {
                _context.Core_Values.Add(core_Values);
                _context.SaveChanges();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
        public void AddBehaviouralStatement(Behavioural_Statement behaviour_Statement)
        {
            try
            {
                _context.Behavioural_Statement.Add(behaviour_Statement);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
        public void AddLearnerValues(Learner_Values values)
        {
            try
            {
                _context.Learner_Values.Add(values);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
        public List<Core_Values> GetAllCoreValues()
        {
            try
            {
                return this.GetDbSet<Core_Values>().ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
        public GradesDetail GetStudentGradeBySubject(int student_Id, int subject_Id)
        {
            try
            {
                var grades = this.GetDbSet<Grades>().Where(g => g.Student_Id == student_Id)
                    .Where(g => g.Subject_Id == subject_Id).Select(g => new GradesViewModel
                    {
                        Grade_Id = g.Grade_Id,
                        Grade = g.Grade,
                        Quarter = g.Quarter,
                    });
                var gradesDetails = new GradesDetail
                {
                    Student_Id = student_Id,
                    Subject_Id = subject_Id,
                    Grades =grades.ToList(),
                };

                return gradesDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public void EditGrade(Grades grade)
        {
            try
            {
                _context.Grades.Update(grade);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public StudentViewModel GetStudent(int student_Id)
        {
            try
            {
                var student= _context.Student.Find(student_Id);
                var userstudent = _context.UsersPortal.Find(student.UID);

                var studentdetails = new StudentViewModel
                {
                    profilePicture = userstudent.ProfilePic,
                    Student_ID = student.Student_Id,
                    FirstName = userstudent.FirstName,
                    MiddleName = userstudent.MiddleName,
                    LastName = userstudent.LastName,
                    age = DateTime.Now.Year - userstudent.Birthday.Year,
                    lrn = student.LRN,
                    Status = student.status,
                    Grade = student.CurrGrade,
                    Birthday = userstudent.Birthday
                };
                return studentdetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public List<string> GetSchoolYears(int student_Id)
        {
            try
            {
                var schoolYearsWithGradeList = new List<string>();
                var schoolYears = this.GetDbSet<Grades>()
                    .Where(g => g.Student_Id == student_Id).ToList().Select(p => p.School_Year).Distinct();
                var grades = this.GetDbSet<Grades>()
                    .Where(g => g.Student_Id == student_Id).ToList();                
                foreach ( var schoolYear in schoolYears)
                {
                    var gradelevel = grades.FirstOrDefault(g => g.School_Year == schoolYear).Grade_Level.ToString();
                    schoolYearsWithGradeList.Add(schoolYear +" Grade:"+ gradelevel);
                }               
                return schoolYearsWithGradeList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public List<string> GetSchoolYearsWithOutGradeLevel(int student_Id)
        {
            try
            {
                var schoolYearsWithGradeList = new List<string>();
                var schoolYears = this.GetDbSet<Grades>()
                    .Where(g => g.Student_Id == student_Id).Select(p => p.School_Year).Distinct();
                var grades = this.GetDbSet<Grades>()
                    .Where(g => g.Student_Id == student_Id);
                foreach (var schoolYear in schoolYears)
                {                    
                    schoolYearsWithGradeList.Add(schoolYear);
                }               
                return schoolYearsWithGradeList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        
        public string GradeLevel(int studentId, string schoolYear)
        {
            try
            {
                var student = this.GetDbSet<Grades>().Where(p => p.Student_Id == studentId && p.School_Year == schoolYear);
                if(student.Count()>0)
                {
                    return student.FirstOrDefault().Grade_Level;
                }
                return _context.Student.Find(studentId).CurrGrade;
            }
             catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public List<StudentGrades> GetChildSubjectGrades(int student_Id, string school_year)
        {
            try
            {
                //Get the grade(S) of a particular student along with the choosen school Year
                var grades = this.GetDbSet<Grades>().Where(g => g.Student_Id == student_Id)
                   .Where(g => g.School_Year == school_year).ToList();
                var subjects = this.GetDbSet<Subject>().ToList();
                //Get the subjects with child subjects
                var HeadSubjects = this.GetDbSet<Subject>().Where(p => p.HasChild == true).ToList();
                //Get the Child Subjects
                var childSubjects = this.GetDbSet<ChildSubject>().ToList();
                //Join the childSubject table to the Subject Table to get the child subject details e.g name                
                var childSubjectsUnion = from s in subjects
                                         join
                                         c in childSubjects on
                                         s.Subject_Id equals c.Subject_Id
                                         join h in HeadSubjects on c.HeadSubjectId equals h.Subject_Id
                                         select new 
                                         {
                                             HeadId = c.HeadSubjectId,
                                             Subject_Id = c.Subject_Id,
                                             Subject_Name = s.Subject_Name,
                                             Grade = s.Grade,
                                             HasChild = s.HasChild,
                                         };
                //Join the Head Subject to the grades table to get its grades.
                var subjectGrade = from g in grades join s in HeadSubjects
                                   on g.Subject_Id equals s.Subject_Id
                                   select new
                                   {
                                       Subject_Id = s.Subject_Id,
                                       SubjectName = s.Subject_Name,
                                       Quarter = g.Quarter,
                                       Grade = g.Grade
                                   };
                //Join the child subject to the grade table to get its grades.
                //The Subject id holds the head id value instead of the child subject id.
                //This is to make sure that the placement in viewing the grades of the head subject and the child subject
                //are adjacent to one another.
                var childSubjectGrade = from g in grades join c in childSubjectsUnion
                                        on g.Subject_Id equals c.Subject_Id
                                        where g.School_Year == school_year
                                        select new
                                        { 
                                            Head_Id = c.HeadId,
                                            Subject_Id = c.Subject_Id,
                                            SubjectName = c.Subject_Name,
                                            Quarter = g.Quarter,
                                            Grade = g.Grade
                                        };
                //Group the Head subject grades
               var subjectGradeGrouped = subjectGrade.GroupBy(g => new { g.SubjectName,g.Subject_Id })
                    .Select(group => new StudentGrades
                    {
                        SubjectId = group.Key.Subject_Id,
                        SubjectName = group.Key.SubjectName,
                        Grades = group.Select(g => new GradesViewModel
                        {
                            Grade = g.Grade,
                            Quarter = g.Quarter,
                        }).OrderBy(p => p.Quarter).ToList()
                    });
                //group the child subject grades
                var childSubjectGradeGrouped = childSubjectGrade.GroupBy(g => new { g.SubjectName, g.Subject_Id,g.Head_Id })
                   .Select(group => new StudentGrades
                   {
                       HeadId = group.Key.Head_Id,
                       SubjectId = group.Key.Subject_Id,
                       SubjectName = group.Key.SubjectName,
                       Grades = group.Select(g => new GradesViewModel
                       {
                           Grade = g.Grade,
                           Quarter = g.Quarter,
                       }).OrderBy(p => p.Quarter).ToList()
                   });
                
                var container = new List<StudentGrades>();
                //To arrange the list of which the Head and Child subjects are contigouse/ not separated.
                foreach(var subject in  subjectGradeGrouped)
                {
                    container.Add(subject);
                    var childSub = childSubjectGradeGrouped.Where(p=> p.HeadId == subject.SubjectId).ToList();
                    foreach(var child in childSub)
                    {
                        container.Add(child);
                    }
                }
                return container;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public List<StudentGrades> GetStudentGrades(int student_Id, string school_year)
        {
            try
            {
                var grades = this.GetDbSet<Grades>().Where(g => g.Student_Id == student_Id)
                    .Where(g=> g.School_Year == school_year);
                var subjects = this.GetDbSet<Subject>().Where(p=> !p.HasChild);
                var headSubjects = this.GetDbSet<HeadSubject>();
                var joinSubs = from s in subjects
                               join h in headSubjects
                               on s.Subject_Id equals h.Subect_Id
                               select new Subject
                               {
                                   Subject_Id = s.Subject_Id,
                                   Subject_Name = s.Subject_Name,
                                   Grade = s.Grade,
                                   HasChild = s.HasChild
                               };
                var studentsubjects = from g in grades join s in joinSubs
                                      on g.Subject_Id equals s.Subject_Id
                                      select new
                                      {
                                          SubjectId = s.Subject_Id,
                                          SubjectName = s.Subject_Name,
                                          Quarter = g.Quarter,
                                          Grade = g.Grade,
                                          ScholasticRecordId = g.ScholasticRecords
                                      };

                var studentGrade = studentsubjects.GroupBy(g => new { g.SubjectName,g.SubjectId, g.ScholasticRecordId})
                    .Select(group => new StudentGrades
                    {
                        HeadId  = 0,
                        SubjectId = group.Key.SubjectId,
                        SubjectName = group.Key.SubjectName,
                        ScholasticRecordId = group.Key.ScholasticRecordId,
                        Grades = group.Select(g => new GradesViewModel
                        {
                            Grade = g.Grade,
                            Quarter = g.Quarter,
                        }).OrderBy(p => p.Quarter).ToList()
                    }).ToList();
                var childSubjectGrades = this.GetChildSubjectGrades(student_Id, school_year).ToList(); // Execute the query to fetch results from the database

                var concatenatedList = studentGrade.Concat(childSubjectGrades).ToList();

                return concatenatedList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public List<ValuesGrades> GetValuesGrades(int StudentId, string schoolyear) 
        {
            try
            {
                var studentGrades = this.GetDbSet<Learner_Values>().Where(p => p.Student_Id == StudentId).Where(p => p.School_Year == schoolyear)
                    .Select(p => new ValuesGrades
                    {
                        Id = p.Id,
                        BehaviouralId = p.Behavioural_Statement,
                        Grade = p.Grade,
                        Quarter = p.Quarter
                        
                    }).ToList();
                
                return studentGrades;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public int GetBehavioralMaxQuarter(int studentId, int BehavioralId, string schoolYear)
        {
            try
            {
                var maxQuarter = 0;
                var list = this.GetDbSet<Learner_Values>().Where(p => p.Student_Id == studentId).Where(p => p.School_Year == schoolYear)
                    .Where(p => p.Behavioural_Statement == BehavioralId);
                if (list.Count() > 0)
                    maxQuarter = list.Max(p => p.Quarter);
                
                return maxQuarter;
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
                var user = this.GetDbSet<UsersPortal>();
                var student = this.GetDbSet<Student>();

                var studentlist = from u in user join s in student
                                  on u.UID equals s.UID select new StudentViewModel
                                  {
                                      Student_ID = s.Student_Id,
                                      FirstName =u.FirstName,
                                      MiddleName = u.MiddleName,
                                      LastName = u.LastName,
                                      age = (DateTime.Today.Year - u.Birthday.Year),
                                      lrn = s.LRN,
                                      Grade = s.CurrGrade,
                                      Status = s.status
                                  };
                return studentlist.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public IEnumerable<StudentPreviewInformation> GetAllStudentPreview()
        {
            try
            {
                var user = this.GetDbSet<UsersPortal>();
                var student = this.GetDbSet<Student>();

                var studentlist = from u in user
                                  join s in student
                                  on u.UID equals s.UID
                                  select new StudentPreviewInformation
                                  {
                                      studentid = s.Student_Id,
                                      fullname = u.FirstName+ " "+u.MiddleName+" "+u.LastName,                                    
                                      age = (DateTime.Today.Year - u.Birthday.Year),
                                      lrn = s.LRN,
                                      grade = s.CurrGrade,
                                      status = s.status
                                  };
                return studentlist;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public List<Learners_Values_Report> GetLearnersValues()
        {
            try
            {
                var core_values = this.GetDbSet<Core_Values>();
                var behavioral = this.GetDbSet<Behavioural_Statement>();

                var learners_Values = from c in core_values
                                      join b in behavioral
                                      on c.Id equals b.Core_Values
                                      select new
                                      {
                                          Core_Values_Id = c.Id,
                                          Core_Values = c.core_Values,
                                          behavioralStatement = b.Statements,
                                          behavioral_Id = b.Id,
                                      };

                var learners_Values_GroupBy = learners_Values.GroupBy(p => new { p.Core_Values, p.Core_Values_Id })
                    .Select(group => new Learners_Values_Report
                    {
                        Core_Values = group.Key.Core_Values,
                        Core_Values_Id= group.Key.Core_Values_Id,
                        behavioural_Statements = group.Select(g => new Behavioural_Statement
                        {
                            Id = g.behavioral_Id,
                            Statements = g.behavioralStatement,
                            Core_Values = g.Core_Values_Id
                        }).ToList()
                    });

                return learners_Values_GroupBy.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public void UpdateCoreValues(Core_Values values)
        {
            try
            {
                _context.Core_Values.Update(values);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public void DeleteCore_Values(Core_Values values)
        {
            try
            {
                _context.Core_Values.Remove(values);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public Behavioural_Statement GetBehavioural_Statement(int id)
        {
            try
            {
                return _context.Behavioural_Statement.Find(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public void DeleteBehavioralStatement(Behavioural_Statement statement)
        {
            try
            {
                _context.Behavioural_Statement.Remove(statement);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public void UpdateBehavioralStatement(Behavioural_Statement statement)
        {
            try
            {
                _context.Behavioural_Statement.Update(statement);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public Behavioural_Statement GetBehaviouralStatementById(int Id)
        {
            try
            {
                return _context.Behavioural_Statement.Find(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public Core_Values GetCoreValuesById(int Id) 
        {
            try
            {
                return _context.Core_Values.Find(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public Learner_Values GetLearnerValuesById(int id)
        {
            try
            {
                return this.GetDbSet<Learner_Values>().Find(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public void UpdateLearnerValues(Learner_Values valuesgrades)
        {
            try
            {
                _context.Learner_Values.Update(valuesgrades);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public void AddAttendance(Attendance attendance)
        {
            try
            {
                _context.Attendance.Add(attendance);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public void UpdateAttendance(Attendance attendance)
        {
            try
            {
                _context.Attendance.Update(attendance);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public void DeleteAttendance(Attendance attendance)
        {
            try
            {
                _context.Attendance.Remove(attendance);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public List<Attendance> GetStudentAtendance(int student_Id, string schoolYear)
        {
            try
            {
                var studentAttendance = this.GetDbSet<Attendance>().Where(p => p.Studentid == student_Id).Where(p => p.School_Year == schoolYear);
                return studentAttendance.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public bool isDateExisting(int month, string schoolYear, int studentid)
        {
            try
            {
                var list = this.GetDbSet<Attendance>().Where(p => p.School_Year == schoolYear).Where(p => p.Month == month).Where(p => p.Studentid == studentid);
                return list.Count() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        
    }
}
