using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class TeacherViewModel
    {
        public string id { get; set; }
        public byte[]? profilepic { get; set; }
        public string firstname { get; set; }   
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
    }
}
