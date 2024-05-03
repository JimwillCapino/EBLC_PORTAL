using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class ValuesWithGradesContainer
    {
        public int classid { get; set; }    
        public int Student_Id { get; set; }
        public string School_Year { get; set; }
        public List<ValuesGrades> Grades { get; set; }
        public List<Learners_Values_Report> Values { get; set; }
        //public List<string> Scool_Years { get; set; }
    }
}
