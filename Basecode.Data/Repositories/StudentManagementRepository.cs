using Basecode.Data.Interfaces;
using Basecode.Data.Models;
using Basecode.Data.ViewModels;
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
        
    }
}
