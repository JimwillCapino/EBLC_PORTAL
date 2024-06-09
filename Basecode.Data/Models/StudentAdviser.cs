using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class StudentAdviser
    {
        public int Id { get; set; }
        public int studentId { get; set; }
        public string AdviserName { get; set; }
        public string Schoolyear { get; set; }
    }
}
