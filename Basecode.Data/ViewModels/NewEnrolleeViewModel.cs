using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class NewEnrolleeViewModel
    {
        public int id { get; set; }        
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }        
        public string sex { get; set; }
        public string gradeenrolled {  get; set; }
        public DateTime birthday { get; set; }
        public string? examschedule { get; set; }
    }
}
