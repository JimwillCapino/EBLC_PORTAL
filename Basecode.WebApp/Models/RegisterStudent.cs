using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class RegisterStudent
    {
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set;}
        public string Phone_Number { get; set; }
        public string Email { get; set; }
        public string gender { get; set; }
        public int age { get; set; }
        public byte[] birthCertificate { get; set; }
        public byte[] COG { get; set; }
        public byte[] TOR { get; set; } 
    }
}
