using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class AdminDashboard
    {
        public string SchoolYear { get; set; }       
        public int TeacherCount { get; set; }
        public int StudentEnrolledCount { get; set; }
        public int StudentNotEnrolledCount { get; set; }
        public int RegistrarsCount { get; set; }
    }
}
