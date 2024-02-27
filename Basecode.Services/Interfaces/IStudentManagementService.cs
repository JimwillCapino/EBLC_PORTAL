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
        public GradesDetail GetStudentGradeBySubject(int student_Id, int subject_Id);
        public void SubmitGrade(int student_id, int Subject_Id, int grade, int Quarter);
        public void EditGrade(int Grade_Id,int student_id, int Subject_Id, int grade, int Quarter);
    }
}
