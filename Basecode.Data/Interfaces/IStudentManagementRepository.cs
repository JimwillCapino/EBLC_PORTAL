using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Interfaces
{
    public interface IStudentManagementRepository
    {
        public void SubmitGrade(Grades grade);
        public GradesDetail GetStudentGradeBySubject(int student_Id, int subject_Id);
        public void EditGrade(Grades grade);
    }
}
