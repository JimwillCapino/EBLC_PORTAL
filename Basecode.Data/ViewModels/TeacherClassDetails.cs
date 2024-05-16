using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class TeacherClassDetails
    {
        public int classid { get; set; }
        public int subjectid { get; set; }
        public string subjectname { get; set; }
        public string classname { get; set; }
        public string grade { get; set; } 
        public bool haschild { get; set; }
        public string schedule { get; set; }
        //public List<ClassStudentViewModel> Students { get; set; }
    }
}
