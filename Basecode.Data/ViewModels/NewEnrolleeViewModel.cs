using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class NewEnrolleeViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Middlename { get; set; }        
        public string sex { get; set; }
        public int GradeEnrolled {  get; set; }
        public DateTime Birthday { get; set; }
        public DateTime? ExamSchedule { get; set; }
    }
}
