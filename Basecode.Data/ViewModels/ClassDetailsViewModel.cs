using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public  class ClassDetailsViewModel
    {
        public List<ClassInitView> Classes {  get; set; }
        public List<TeacherViewModel> Teachers { get; set; }
    }
}
