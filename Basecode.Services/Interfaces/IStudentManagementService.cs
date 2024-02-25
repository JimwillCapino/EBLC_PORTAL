using Basecode.Data.Models;
using Basecode.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Services.Interfaces
{
    public interface IStudentManagementService
    {
        public List<GradesViewModel> GetStudentGradeBySubject(int student_Id, int subject_Id);
        public void SubmitGrade(Grades grades);
    }
}
