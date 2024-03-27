using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class HomeRoom
    {
        public int Class_Id { get; set; }
        public string ClassName { get; set; }
        public int Grade { get; set; }
        public List<ClassStudentViewModel> Students { get; set; }
    }
}
