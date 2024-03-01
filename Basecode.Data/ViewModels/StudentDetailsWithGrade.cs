using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class StudentDetailsWithGrade
    {
        public List<string> School_Years { get; set; }
        public StudentViewModel Student { get; set; }
        public List<StudentGrades> grades { get; set; }
    }
}
