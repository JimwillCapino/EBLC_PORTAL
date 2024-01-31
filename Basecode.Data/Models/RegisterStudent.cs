using Microsoft.AspNetCore.Http;
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
        public int UID { get; set; }    
        public int Enrollee_Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set;}
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string email { get; set; }
        public string sex { get; set; }
        public int age { get; set; }
        public IFormFile? BirthCertificateFile { get; set; }
        public IFormFile? CGMFile { get; set; }
        public IFormFile? TORFile { get; set; }
        public byte[]? BirthCertificateRecieve { get; set; }
        public byte[]? CGMRecieve { get; set; }
        public byte[]? TORRecieve { get; set; }
    }
}
