using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class ClassSubjectViewModel
    {
        public int Id { get; set; } 
        public int Subject_Id { get; set; }
        public string TeacherId {  get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
        public string Schedule { get; set; }
    }
}
