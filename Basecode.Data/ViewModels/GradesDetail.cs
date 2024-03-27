using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class GradesDetail
    {
        public int Student_Id { get; set; }
        public int Subject_Id { get; set; }
        public List<GradesViewModel> Grades { get; set; }
    }
}
