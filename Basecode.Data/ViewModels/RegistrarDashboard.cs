using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class RegistrarDashboard
    {
        public string SchoolYear { get; set; }
        public int NewEnrolleeCount { get; set; }
        public int TeacherCount { get; set; }
        public int StudentEnrolledCount { get; set; }
        public int StudentNotEnrolledCount { get; set; }
        public List<NewEnrolleeViewModel> NewEnrolleeList { get; set; }
    }
}
