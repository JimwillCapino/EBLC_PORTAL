using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class StudentGrades
    {
        public int HeadId { get; set; }
        public int SubjectId { get; set; }  
        public string SubjectName { get; set; }
        public List<GradesViewModel> Grades { get; set; }
    }
}
