using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class ClassInitView
    {
        public int Id { get; set; }
        public string AdviserId { get; set; }
        public int Grade { get; set; }
        public int ClassSize { get; set; }
        public string? ClassName { get; set; }
        public string AdviserName { get; set; }
    }
}
