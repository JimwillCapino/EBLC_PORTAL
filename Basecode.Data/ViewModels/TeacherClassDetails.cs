using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class TeacherClassDetails
    {
        public int Class_Id { get; set; }
        public int Subject_Id { get; set; }
        public string Subject_Name { get; set; }
        public string Class_Name { get; set; }
        public int grade { get; set; } 
        public bool HasChild { get; set; }
        public List<ClassStudentViewModel> Students { get; set; }
    }
}
