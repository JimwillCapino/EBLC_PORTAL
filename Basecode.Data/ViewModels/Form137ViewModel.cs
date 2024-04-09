using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class Form137ViewModel
    {
        public int GradeLevel { get; set; }
        public string SchoolYear { get; set; }
        public int TotalHeadSubjectCount { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<StudentGrades> grades { get; set; }       
    }
}
