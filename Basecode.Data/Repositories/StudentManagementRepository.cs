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
                    Student_ID = student.Student_Id,
                    FirstName = userstudent.FirstName,
                    MiddleName = userstudent.MiddleName,
                    LastName = userstudent.LastName,
                    age = DateTime.Now.Year - userstudent.Birthday.Year,
                    lrn = student.LRN,
                    Status = student.status,
                    Grade = student.CurrGrade
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
                var schoolYears = this.GetDbSet<Grades>()
                    .Where(g => g.Student_Id == student_Id)
                    .Select(g => g.School_Year).Distinct();
                return schoolYears.ToList();
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
                var subjects = this.GetDbSet<Subject>();

                var studentsubjects = from g in grades join s in subjects
                                      on g.Subject_Id equals s.Subject_Id
                                      select new
                                      {
                                          SubjectName = s.Subject_Name,
                                          Quarter = g.Quarter,
                                          Grade = g.Grade
                                      };

                var studentGrade = studentsubjects.GroupBy(g => g.SubjectName)
                    .Select(group => new StudentGrades
                    {
                        SubjectName = group.Key,
                        Grades = group.Select(g => new GradesViewModel
                        {
                            Grade = g.Grade,
                            Quarter = g.Quarter,
                        }).ToList()
                    });
                return studentGrade.ToList();
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
    }
}
