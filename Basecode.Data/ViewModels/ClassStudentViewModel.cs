using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class ClassStudentViewModel
    {
        public int id { get; set; }
        public int studentid { get; set; }        
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string? middlename { get; set; }
    }
}
