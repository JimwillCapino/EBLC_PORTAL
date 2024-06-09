using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class StudentDetailsContainer
    {
        public StudentViewModel Student { get; set; }
        public ParentDetails Parent { get; set; }
    }
}
