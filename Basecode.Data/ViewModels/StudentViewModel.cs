
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class StudentViewModel
    {
        public int Student_ID { get; set; } 
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        public DateTime Birthday { get; set; }
        public string sex { get; set; }
        public byte[]? profilePicture { get; set; }
        public int age { get; set; }
        public int Grade { get; set; }
        public string lrn { get; set; }
    }
}
