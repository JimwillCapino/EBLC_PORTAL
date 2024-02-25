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
        public List<GradesViewModel> GetStudentGradeBySubject(int student_Id, int subject_Id)
        {
            try
            {
                var grades = this.GetDbSet<Grades>().Where(g => g.Student_Id == student_Id)
                    .Where(g => g.Subject_Id == subject_Id).Select(g => new GradesViewModel
                    {
                        Grade = g.Grade,
                        Quarter = g.Quarter,
                    });
                return grades.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
