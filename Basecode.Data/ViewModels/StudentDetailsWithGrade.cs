using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class StudentDetailsWithGrade
    {
        public int TotalHeadSubjectCount { get; set; }
        public ClassInitView studentClass { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<string> School_Years { get; set; }               
        public StudentViewModel Student { get; set; }
        public List<StudentGrades> grades { get; set; }
        public List<ValuesGrades> valuesGrades { get; set; }
        public List<Learners_Values_Report> learnersValues { get; set; }
        public AttendanceContainer StudentAttendance { get; set; }
        public string SchoolYear { get; set; }
        public ParentDetails Parent { get; set; }
        public  int? PassingGrade { get; set; }
    }
}
