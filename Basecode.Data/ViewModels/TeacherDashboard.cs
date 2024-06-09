using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class TeacherDashboard
    {
        public string SchoolYear { get; set; }
        public string SubjectName { get; set; }
        public int NumberOfClass { get; set; }
        public int NumberOfHomeroom { get; set; }
        public List<ClassStudentViewModel> ListOfStudentsWithNoGrade { get; set; }
        public List<TeacherClassDetails> ClassesOfTeacher { get; set; }       
    }
}
