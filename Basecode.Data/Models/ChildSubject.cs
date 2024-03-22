using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class ChildSubject
    {
        public int Id { get; set; }
        public int HeadSubjectId { get; set; }
        public int Subject_Id { get; set; }
    }
}
