using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class SubjectViewModel
    {
        public int subjectid { get; set; }
        public string subjectname { get; set; }
        public int grade { get; set; }
        public bool haschild { get; set; }
    }
}
